using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly IPlaybackRequestHandler<GetPlaybackInfoRequest, GetPlaybackRequestResult> _playbackInfoRequestHandler;
        private static readonly ConcurrentDictionary<string, SocketConnection> Connections = new();

        public PlaybackSocketDirector(IPlaybackRequestHandler<GetPlaybackInfoRequest, GetPlaybackRequestResult> playbackInfoRequestHandler)
        {
            _playbackInfoRequestHandler = playbackInfoRequestHandler;
        }
        
        public async Task HandleSocketConnection(WebSocket socket, HttpContext context)
        {
            var socketConnection = new SocketConnection(socket, context);
            if (Connections.TryAdd(Guid.NewGuid().ToString(), socketConnection))
            {
                var playbackInfo = await _playbackInfoRequestHandler.HandleAsync(new GetPlaybackInfoRequest());
                var response = new PlaybackSocketMessage<string>()
                {
                    Body = playbackInfo.Message,
                    MessageType = MessageType.Playback,
                    ContentType = PlaybackMessageContentType.PlaybackUpdate,
                };
                await socketConnection.SendMessage(response.GetBytes());
                await socketConnection.PollConnection(async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        // TODO: handle message or maybe delete receiving messages in the future
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