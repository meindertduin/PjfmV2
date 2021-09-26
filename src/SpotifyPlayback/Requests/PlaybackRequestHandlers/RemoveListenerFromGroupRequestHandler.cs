using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class RemoveListenerFromGroupRequestHandler : IPlaybackRequestHandler<RemoveListenerFromGroupRequest,
        PlaybackRequestResult<RemoveListenerFromGroupRequestResult>>
    {
        private readonly ISpotifyPlaybackService _spotifyPlaybackService;
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly ISocketConnectionCollection _socketConnectionCollection;

        public RemoveListenerFromGroupRequestHandler(ISpotifyPlaybackService spotifyPlaybackService,
            IPlaybackGroupCollection playbackGroupCollection, ISocketConnectionCollection socketConnectionCollection)
        {
            _spotifyPlaybackService = spotifyPlaybackService;
            _playbackGroupCollection = playbackGroupCollection;
            _socketConnectionCollection = socketConnectionCollection;
        }

        public Task<PlaybackRequestResult<RemoveListenerFromGroupRequestResult>> HandleAsync(
            RemoveListenerFromGroupRequest request)
        {
            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(request.UserGroupId);
            playbackGroup.RemoveListener(request.ConnectionId);
            
            _spotifyPlaybackService.PausePlaybackForUser(request.UserId);
            var socketConnection = _socketConnectionCollection.GetSocketConnection(request.ConnectionId);
            if (socketConnection == null)
            {
                throw new NullReferenceException();
            }
            socketConnection.ClearListeningPlaybackGroupId();

            return PlaybackRequestResult.SuccessAsync(new RemoveListenerFromGroupRequestResult(), "Successfully removed user.");
        }
    }

    public class
        RemoveListenerFromGroupRequest : IPlaybackRequest<PlaybackRequestResult<RemoveListenerFromGroupRequestResult>>
    {
        public string UserId { get; set; } = null!;
        public Guid UserGroupId { get; set; } = default!;
        public Guid ConnectionId { get; set; } = default!;
    }

    public class RemoveListenerFromGroupRequestResult
    {
    }
};