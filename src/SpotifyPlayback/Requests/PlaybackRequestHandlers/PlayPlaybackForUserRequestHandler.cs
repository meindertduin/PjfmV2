using System;
using System.Threading.Tasks;
using Pjfm.Common.Authentication;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class PlayPlaybackForUserRequestHandler : IPlaybackRequestHandler<PlayPlaybackForUserRequest,
        PlaybackRequestResult<PlayPlaybackForUserRequestResult>>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly ISpotifyPlaybackService _spotifyPlaybackService;
        private readonly ISocketConnectionCollection _socketConnectionCollection;

        public PlayPlaybackForUserRequestHandler(IPlaybackGroupCollection playbackGroupCollection,
            ISpotifyPlaybackService spotifyPlaybackService, ISocketConnectionCollection socketConnectionCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _spotifyPlaybackService = spotifyPlaybackService;
            _socketConnectionCollection = socketConnectionCollection;
        }

        public Task<PlaybackRequestResult<PlayPlaybackForUserRequestResult>> HandleAsync(
            PlayPlaybackForUserRequest request)
        {
            var retrievedConnectionId =
                _socketConnectionCollection.TryGetUserSocketConnection(request.Principal.Id, out var socketConnection);
            if (!retrievedConnectionId)
            {
                return Task.FromResult(
                    PlaybackRequestResult.Fail<PlayPlaybackForUserRequestResult>("Failed to retrieve ConnectionId."));
            }

            var newListener = new ListenerDto(socketConnection!.ConnectionId, request.Principal, request.DeviceId);

            var hasJoinedAsListener = _playbackGroupCollection.ListenToGroup(request.GroupId, newListener);
            if (!hasJoinedAsListener)
            {
                return Task.FromResult(
                    PlaybackRequestResult.Fail<PlayPlaybackForUserRequestResult>("Failed to join as listener."));
            }

            _socketConnectionCollection.SetSocketConnectedGroupId(socketConnection.ConnectionId, request.GroupId);

            var groupInfo = _playbackGroupCollection.GetPlaybackGroupInfo(request.GroupId);
            if (groupInfo.CurrentlyPlayingTrack != null)
            {
                var trackStartTimeMs =
                    (DateTime.Now - groupInfo.CurrentlyPlayingTrack.TrackStartDate).TotalMilliseconds;
                _spotifyPlaybackService.PlayTrackForUser(newListener, groupInfo.CurrentlyPlayingTrack.SpotifyTrackId,
                    request.SpotifyAccessToken, (int) trackStartTimeMs);
            }

            return Task.FromResult(
                PlaybackRequestResult.Success(new PlayPlaybackForUserRequestResult(),
                    "User joined group successfully"));
        }
    }

    public class PlayPlaybackForUserRequest : IPlaybackRequest<PlaybackRequestResult<PlayPlaybackForUserRequestResult>>
    {
        public Guid GroupId { get; set; }
        public string DeviceId { get; set; } = null!;
        public IPjfmPrincipal Principal { get; set; } = null!;
        public string SpotifyAccessToken { get; set; } = null!;
    }

    public class PlayPlaybackForUserRequestResult
    {
    }
}