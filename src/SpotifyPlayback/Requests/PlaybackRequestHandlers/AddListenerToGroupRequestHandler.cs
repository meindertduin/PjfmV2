using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class AddListenerToGroupRequestHandler : IPlaybackRequestHandler<AddListenerToGroupRequest, PlaybackRequestResult<AddListenerToGroupRequestResult>>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly ISpotifyPlaybackService _spotifyPlaybackService;

        public AddListenerToGroupRequestHandler(IPlaybackGroupCollection playbackGroupCollection, ISpotifyPlaybackService spotifyPlaybackService)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _spotifyPlaybackService = spotifyPlaybackService;
        }
        
        public Task<PlaybackRequestResult<AddListenerToGroupRequestResult>> HandleAsync(AddListenerToGroupRequest request)
        {
            var groupInfo = _playbackGroupCollection.GetPlaybackGroupInfo(request.GroupId);

            if (groupInfo.CurrentlyPlayingTrack != null)
            {
                var trackStartTimeMs = (DateTime.Now - groupInfo.CurrentlyPlayingTrack.TrackStartDate).TotalMilliseconds;
                _spotifyPlaybackService.PlayTrackForUser(request.NewListener, groupInfo.CurrentlyPlayingTrack.SpotifyTrackId, (int) trackStartTimeMs);
            }

            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(request.GroupId);
            var hasJoinedAsListener = playbackGroup.AddListener(request.NewListener);
            
            if (!hasJoinedAsListener)
            {
                return Task.FromResult(PlaybackRequestResult.Fail<AddListenerToGroupRequestResult>("Failed to join playbackgroup."));
            }

            return Task.FromResult(PlaybackRequestResult.Success(new AddListenerToGroupRequestResult(), "User successfully joined playbackgroup."));
        }
    }

    public class AddListenerToGroupRequest : IPlaybackRequest<PlaybackRequestResult<AddListenerToGroupRequestResult>>
    {
        public string GroupId { get; set; } = default!;
        public ListenerDto NewListener { get; set; } = null!;
    }

    public class AddListenerToGroupRequestResult
    {
        
    }
}