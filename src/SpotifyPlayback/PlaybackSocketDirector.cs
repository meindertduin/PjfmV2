using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Requests.SocketRequestHandlers;

namespace SpotifyPlayback
{
    public class PlaybackSocketDirector : ISocketDirector
    {
        private readonly IPlaybackRequestDispatcher _playbackRequestDispatcher;
        private readonly ISocketRequestHandler _socketRequestHandler;
        private static readonly ConcurrentDictionary<Guid, SocketConnection> Connections = new();
        private static readonly ConcurrentDictionary<string, Guid> UserConnectionIdMap = new();

        public PlaybackSocketDirector(IPlaybackRequestDispatcher playbackRequestDispatcher, ISocketRequestHandler socketRequestHandler)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
            _socketRequestHandler = socketRequestHandler;
        }
        
        public async Task HandleSocketConnection(WebSocket socket, HttpContext context)
        {
            var socketConnection = new SocketConnection(socket, context, Guid.NewGuid());
            if (Connections.TryAdd(socketConnection.ConnectionId, socketConnection))
            {
                socketConnection.UpdateConnectionStatus();
                
                var response = new PlaybackSocketMessage<int>()
                {
                    MessageType = MessageType.ConnectionEstablished,
                };

                await socketConnection.SendMessage(response.GetBytes());

                await socketConnection.PollConnection((result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        _socketRequestHandler.HandleSocketRequest(buffer, socketConnection);
                    }
                });

                // If the connection isn't polling anymore it means that the connection is likely closed
                await HandleConnectionClose(socketConnection);
            }
        }

        private async Task HandleConnectionClose(SocketConnection socketConnection)
        {
            if (!socketConnection.IsConnected)
            {
                if (socketConnection.Principal.IsAuthenticated())
                {
                    await _playbackRequestDispatcher.HandlePlaybackSocketRequest(new DisconnectPlaybackGroupRequest(), socketConnection);
                }

                RemoveUserFromConnectionIdMap(socketConnection);
                RemoveSocket(socketConnection.ConnectionId);
            }
        }

        private void RemoveUserFromConnectionIdMap(SocketConnection socketConnection)
        {
            if (socketConnection.Principal.IsAuthenticated())
            {
                UserConnectionIdMap.Remove(socketConnection.Principal.Id, out _);
            }
        }

        public bool RemoveSocket(Guid connectionId)
        {
            return Connections.Remove(connectionId, out _);
        }

        public IEnumerable<SocketConnection> GetSocketConnections()
        {
            return Connections.Values;
        }

        public bool TryGetUserSocketConnection(string userId, [MaybeNullWhen(false)] out SocketConnection socketConnection)
        {
            var hasFoundConnectionId = UserConnectionIdMap.TryGetValue(userId, out var connectionId);
            if (!hasFoundConnectionId)
            {
                socketConnection = null;
                return false;
            }
            var hasFoundSocketConnection = Connections.TryGetValue(connectionId, out socketConnection);
            return hasFoundSocketConnection;
        }

        public async Task BroadCastMessage<T>(SocketMessage<T> message)
        {
            await SendMessageToConnections(message, Connections.Values);
        }

        public async Task BroadCastMessageOverConnections<T>(SocketMessage<T> message, IEnumerable<Guid> connectionIds)
        {
            var connections = Connections
                .Where(c => connectionIds.Contains(c.Key))
                .Select(c => c.Value);
            
            await SendMessageToConnections(message, connections);
        }

        private async Task SendMessageToConnections<T>(SocketMessage<T> message,
            IEnumerable<SocketConnection> socketConnections)
        {
            var messageBytes = message.GetBytes();
            var sendMessageTasks = new List<Task>();
            
            foreach (var connection in socketConnections)
            {
                sendMessageTasks.Add(connection.SendMessage(messageBytes));
            }

            await Task.WhenAll(sendMessageTasks);
        }
    }
}