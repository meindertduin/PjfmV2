using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class JoinPlaybackGroupRequestHandler : IPlaybackRequestHandler<JoinPlaybackGroupRequest>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public JoinPlaybackGroupRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        public Task HandleAsync(JoinPlaybackGroupRequest request, SocketConnection socketConnection)
        {
            var luistenaar = new ListenerDto(request.UserId, request.ConnectionId);
            var hasJoined = _playbackGroupCollection.JoinGroup(request.GroupId, luistenaar);
            
            return Task.CompletedTask;
        }
    }

    public class JoinPlaybackGroupRequest : IPlaybackRequest
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; } = null!;
        public Guid ConnectionId { get; set; }
    }
}