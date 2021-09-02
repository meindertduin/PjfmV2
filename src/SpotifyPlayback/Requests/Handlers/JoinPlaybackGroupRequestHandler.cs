using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class JoinPlaybackGroupRequestHandler : IPlaybackRequestHandler<JoinPlaybackGroupRequest, JoinPlaybackGroupResult>
    {
        private readonly IPlaybackGroepCollection _playbackGroepCollection;

        public JoinPlaybackGroupRequestHandler(IPlaybackGroepCollection playbackGroepCollection)
        {
            _playbackGroepCollection = playbackGroepCollection;
        }
        public Task<JoinPlaybackGroupResult> HandleAsync(JoinPlaybackGroupRequest request)
        {
            var luistenaar = new ListenerDto(request.UserId, request.ConnectionId);
            var hasJoined = _playbackGroepCollection.JoinGroup(request.GroupId, luistenaar);
            
            return Task.FromResult(new JoinPlaybackGroupResult()
            {
                Success = hasJoined,
            });
        }
    }

    public class JoinPlaybackGroupRequest : IPlaybackRequest<JoinPlaybackGroupResult>
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; } = null!;
        public Guid ConnectionId { get; set; }
    }

    public class JoinPlaybackGroupResult
    {
        public bool Success { get; set; }
    }
}