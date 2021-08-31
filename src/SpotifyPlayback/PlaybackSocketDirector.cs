using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
                var response = new PlaybackSocketMessage<GetPlaybackInfoRequestResult>()
                {
                    Body = playbackInfo,
                    MessageType = MessageType.Playback,
                    ContentType = PlaybackMessageContentType.PlaybackUpdate,
                };
                await socketConnection.SendMessage(response.GetBytes());
                await socketConnection.PollConnection((result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        // TODO: handle message or maybe delete receiving messages in the future
                    }
                });

                if (!socketConnection.IsConnected)
                {
                    if (socketConnection.Principal.IsAuthenticated())
                    {
                        await _playbackRequestDispatcher.HandlePlaybackRequest(new DisconnectPlaybackGroupRequest()
                        {
                            GebruikerId = socketConnection.Principal.Id,
                        });
                    }
                    
                    RemoveSocket(socketConnection.ConnectionId);
                }
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