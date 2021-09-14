using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Requests.SocketRequestHandlers;

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
            var serializedObject = JsonConvert.DeserializeObject<SocketRequest<dynamic>>(json);

            if (serializedObject == null)
            {
                return Task.CompletedTask;
            }

            return serializedObject.RequestType switch
            {
                RequestType.ConnectToGroup => SendRequestThroughDispatcher<JoinPlaybackGroupSocketRequest>(
                    serializedObject.Body, socketConnection),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Task SendRequestThroughDispatcher<T>(JObject request, SocketConnection socketConnection)
            where T : IPlaybackRequest
        {
            var playbackRequest = JsonConvert.DeserializeObject<T>(request.ToString());
            if (playbackRequest != null)
            {
                return _playbackRequestDispatcher.HandlePlaybackSocketRequest(playbackRequest, socketConnection);
            }

            throw new JsonSerializationException($"Cannot serialize request to type: {typeof(T)}");
        }
    }
}