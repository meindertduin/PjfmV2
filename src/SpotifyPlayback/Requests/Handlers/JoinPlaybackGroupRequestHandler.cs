using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class JoinPlaybackGroupRequestHandler : IPlaybackRequestHandler<JoinPlaybackGroupRequest, JoinPlaybackGroupResult>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public JoinPlaybackGroupRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        public Task<JoinPlaybackGroupResult> HandleAsync(JoinPlaybackGroupRequest request)
        {
            var luistenaar = new LuisteraarDto(request.GebruikerId, request.ConnectionId);
            var hasJoined = _playbackGroupCollection.JoinGroup(request.GroupId, luistenaar);
            
            return Task.FromResult(new JoinPlaybackGroupResult()
            {
                Success = hasJoined,
            });
        }
    }

    public class JoinPlaybackGroupRequest : IPlaybackRequest<JoinPlaybackGroupResult>
    {
        public Guid GroupId { get; set; }
        public string GebruikerId { get; set; }
        public Guid ConnectionId { get; set; }
    }

    public class JoinPlaybackGroupResult
    {
        public bool Success { get; set; }
    }
}