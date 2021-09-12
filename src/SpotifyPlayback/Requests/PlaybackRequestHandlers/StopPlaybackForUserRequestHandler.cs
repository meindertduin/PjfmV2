using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class StopPlaybackForUserRequestHandler : IPlaybackRequestHandler<StopPlaybackForUserRequest>
    {
        private readonly ISocketDirector _socketDirector;
        private readonly ISpotifyPlaybackService _spotifyPlaybackService;
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public StopPlaybackForUserRequestHandler(ISocketDirector socketDirector,
            ISpotifyPlaybackService spotifyPlaybackService, IPlaybackGroupCollection playbackGroupCollection)
        {
            _socketDirector = socketDirector;
            _spotifyPlaybackService = spotifyPlaybackService;
            _playbackGroupCollection = playbackGroupCollection;
        }

        public Task HandleAsync(StopPlaybackForUserRequest request)
        {
            if (!_socketDirector.TryGetUserSocketConnection(request.UserId, out var socketConnection))
            {
                throw new NullReferenceException();
            }

            var userGroupId = socketConnection.GetConnectedPlaybackGroupId();
            if (userGroupId != null)
            {
                _playbackGroupCollection.RemoveListenerFromGroup(socketConnection.ConnectionId, userGroupId.Value);
                _spotifyPlaybackService.PausePlaybackForUser(request.UserId);
                _socketDirector.ClearSocketConnectedGroupId(socketConnection.ConnectionId);
                
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