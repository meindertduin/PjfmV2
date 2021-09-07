using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Requests.Handlers;

namespace SpotifyPlayback.Services
{
    public class SocketRequestHandler : ISocketRequestHandler
    {
        private readonly IPlaybackRequestDispatcher _playbackRequestDispatcher;

        public SocketRequestHandler(IPlaybackRequestDispatcher playbackRequestDispatcher)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
        }

        public Task HandleSocketRequest(byte[] buffer, SocketConnection socketConnection)
        {
            var json = Encoding.UTF8.GetString(buffer);
            var serializedObject =
                JsonConvert.DeserializeObject<SocketRequest<object>>(json, new JsonSerializerSettings());

            // TODO: handle badRequests accordingly
            if (serializedObject == null)
            {
                return Task.CompletedTask;
            }

            switch (serializedObject.RequestType)
            {
                case RequestType.ConnectToGroup:
                    return SendRequestThroughDispatcher<JoinPlaybackGroupRequest>(serializedObject.Body, socketConnection);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Task SendRequestThroughDispatcher<T>(object request, SocketConnection socketConnection) where T : IPlaybackRequest
        {
            if (request is T playbackRequest)
            {
                return _playbackRequestDispatcher.HandlePlaybackSocketRequest(playbackRequest, socketConnection);
            }

            throw new InvalidCastException($"Cannot cast request to type: {typeof(T)}");
        }
    }
}