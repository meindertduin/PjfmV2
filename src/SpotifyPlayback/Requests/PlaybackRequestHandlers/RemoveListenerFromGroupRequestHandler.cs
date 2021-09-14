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
            _playbackGroupCollection.RemoveListenerFromGroup(request.ConnectionId, request.UserGroupId);
            _spotifyPlaybackService.PausePlaybackForUser(request.UserId);
            _socketConnectionCollection.ClearSocketConnectedGroupId(request.ConnectionId);

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