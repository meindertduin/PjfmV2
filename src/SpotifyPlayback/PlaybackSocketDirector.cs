using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback
{
    public class PlaybackSocketDirector : ISocketDirector
    {
        private static readonly ConcurrentDictionary<string, SocketConnection> Connections = new();
        
        public async Task HandleSocketConnection(WebSocket socket, HttpContext context)
        {
            var socketConnection = new SocketConnection(socket, context);
            if (Connections.TryAdd(Guid.NewGuid().ToString(), socketConnection))
            {
                await socketConnection.PollConnection(async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var jsonString = Encoding.UTF8.GetString(buffer);

                        var response = new PlaybackSocketMessage<string>()
                        {
                            MessageType = MessageType.Playback,
                            Body = "ok",
                            ContentType = PlaybackMessageContentType.PlaybackUpdate,
                        };
                        await socket.SendAsync(new ArraySegment<byte>(response.GetBytes()), WebSocketMessageType.Text, true,
                            CancellationToken.None);
                    }
                });
            }
        }

        public bool RemoveSocket(string gebruikerId)
        {
            return Connections.Remove(gebruikerId, out _);
        }

        public IEnumerable<SocketConnection> GetSocketConnections()
        {
            return Connections.Values;
        }
    }
}