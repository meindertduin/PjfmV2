using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class StopPlaybackForUserRequestHandler : IPlaybackRequestHandler<StopPlaybackForUserRequest>
    {
        private readonly ISpotifyPlaybackService _spotifyPlaybackService;
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly ISocketConnectionCollection _socketConnectionCollection;

        public StopPlaybackForUserRequestHandler(ISpotifyPlaybackService spotifyPlaybackService, IPlaybackGroupCollection playbackGroupCollection,
            ISocketConnectionCollection socketConnectionCollection)
        {
            _spotifyPlaybackService = spotifyPlaybackService;
            _playbackGroupCollection = playbackGroupCollection;
            _socketConnectionCollection = socketConnectionCollection;
        }

        public Task HandleAsync(StopPlaybackForUserRequest request)
        {
            if (!_socketConnectionCollection.TryGetUserSocketConnection(request.UserId, out var socketConnection))
            {
                throw new NullReferenceException();
            }

            var userGroupId = socketConnection.GetConnectedPlaybackGroupId();
            if (userGroupId != null)
            {
                _playbackGroupCollection.RemoveListenerFromGroup(socketConnection.ConnectionId, userGroupId.Value);
                _spotifyPlaybackService.PausePlaybackForUser(request.UserId);
                _socketConnectionCollection.ClearSocketConnectedGroupId(socketConnection.ConnectionId);

                return Task.CompletedTask;
            }

            throw new NullReferenceException();
        }
    }

    public class StopPlaybackForUserRequest : IPlaybackRequest
    {
        public string UserId { get; set; } = null!;
    }
}