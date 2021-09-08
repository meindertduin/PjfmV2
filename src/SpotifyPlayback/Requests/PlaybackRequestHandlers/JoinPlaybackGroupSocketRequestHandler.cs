using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class
        JoinPlaybackGroupSocketRequestHandler : IPlaybackRequestHandler<JoinPlaybackGroupRequest, JoinPlaybackGroupResult>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly ISocketDirector _socketDirector;

        public JoinPlaybackGroupSocketRequestHandler(IPlaybackGroupCollection playbackGroupCollection,
            ISocketDirector socketDirector)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _socketDirector = socketDirector;
        }

        public Task<JoinPlaybackGroupResult> HandleAsync(JoinPlaybackGroupRequest request)
        {
            var listener = new ListenerDto(request.UserId, request.ConnectionId);
            var hasJoined = _playbackGroupCollection.JoinGroup(request.GroupId, listener);

            return Task.FromResult(new JoinPlaybackGroupResult()
            {
                SuccessfullyJoined = hasJoined,
            });
        }
    }

    public class JoinPlaybackGroupRequest : IPlaybackRequest<JoinPlaybackGroupResult>
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; }
        public Guid ConnectionId { get; set; }
    }

    public class JoinPlaybackGroupResult
    {
        public bool SuccessfullyJoined { get; set; }
    }
}