using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IdentityServer4.Stores.Serialization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Requests.Handlers;

namespace SpotifyPlayback
{
    public class PlaybackSocketDirector : ISocketDirector
    {
        private readonly IPlaybackRequestDispatcher _playbackRequestDispatcher;
        private static readonly ConcurrentDictionary<Guid, SocketConnection> Connections = new();

        public PlaybackSocketDirector(IPlaybackRequestDispatcher playbackRequestDispatcher)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
        }
        
        public async Task HandleSocketConnection(WebSocket socket, HttpContext context)
        {
            var socketConnection = new SocketConnection(socket, context, Guid.NewGuid());
            if (Connections.TryAdd(socketConnection.ConnectionId, socketConnection))
            {
                var playbackInfo = await _playbackRequestDispatcher.HandlePlaybackRequest(new GetPlaybackInfoRequest());
                var response = new PlaybackSocketMessage<int>()
                {
                    MessageType = MessageType.ConnectionEstablished,
                };

                socketConnection.UpdateConnectionStatus();
                await socketConnection.SendMessage(response.GetBytes());

                await socketConnection.PollConnection((result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        // TODO: handle message or maybe delete receiving messages in the future

                        var json = Encoding.UTF8.GetString(buffer);
                        var serializedObject =
                            JsonConvert.DeserializeObject<dynamic>(json, new JsonSerializerSettings());

                        var requestType = (RequestType) ((serializedObject?.requestType ?? null) ?? throw new InvalidOperationException());
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
                    await _playbackRequestDispatcher.HandlePlaybackRequest(new DisconnectPlaybackGroupRequest()
                    {
                        UserId = socketConnection.Principal.Id,
                    });
                }

                RemoveSocket(socketConnection.ConnectionId);
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