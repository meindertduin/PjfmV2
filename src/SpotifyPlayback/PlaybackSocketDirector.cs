using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pjfm.Common.Authentication;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Requests.Handlers;

namespace SpotifyPlayback
{
    public class PlaybackSocketDirector : ISocketDirector
    {
        private readonly IPlaybackRequestHandler<GetPlaybackInfoRequest, GetPlaybackInfoRequestResult> _playbackInfoRequestHandler;
        private static readonly ConcurrentDictionary<string, SocketConnection> Connections = new();

        public PlaybackSocketDirector(IPlaybackRequestHandler<GetPlaybackInfoRequest, GetPlaybackInfoRequestResult> playbackInfoRequestHandler)
        {
            _playbackInfoRequestHandler = playbackInfoRequestHandler;
        }
        
        public async Task HandleSocketConnection(WebSocket socket, HttpContext context)
        {
            var socketConnection = new SocketConnection(socket, context);
            if (Connections.TryAdd(context.User.GetPjfmPrincipal().Id, socketConnection))
            {
                var playbackInfo = await _playbackInfoRequestHandler.HandleAsync(new GetPlaybackInfoRequest());
                var response = new PlaybackSocketMessage<GetPlaybackInfoRequestResult>()
                {
                    Body = playbackInfo,
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

        public async Task BroadCastMessage<T>(SocketMessage<T> message)
        {
            await SendMessageToConnections(message, Connections.Values);
        }

        public async Task BroadCastMessageOverUsers<T>(SocketMessage<T> message, IEnumerable<string> gebruikerIds)
        {
            var connections = Connections
                .Where(c => gebruikerIds.Contains(c.Key))
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