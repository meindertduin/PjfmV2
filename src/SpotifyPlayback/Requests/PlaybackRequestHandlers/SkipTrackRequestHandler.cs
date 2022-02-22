using System;
using System.Linq;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class SkipTrackRequestHandler : IPlaybackRequestHandler<SkipTrackRequest,
            PlaybackRequestResult<SkipTrackRequestResult>>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly IPlaybackScheduledTrackQueue _playbackScheduledTrackQueue;
        private readonly ISpotifyPlaybackController _spotifyPlaybackController;

        public SkipTrackRequestHandler(IPlaybackGroupCollection playbackGroupCollection,
            IPlaybackScheduledTrackQueue playbackScheduledTrackQueue,
            ISpotifyPlaybackController spotifyPlaybackController)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _playbackScheduledTrackQueue = playbackScheduledTrackQueue;
            _spotifyPlaybackController = spotifyPlaybackController;
        }

        public async Task<PlaybackRequestResult<SkipTrackRequestResult>> HandleAsync(SkipTrackRequest request)
        {
            var groupNewTrack = await _playbackGroupCollection.GetGroupNewTrack(request.GroupId);

            var nextTrack = _playbackScheduledTrackQueue.GetScheduledTracks()
                .FirstOrDefault(t => t.GroupId == request.GroupId);
            
            if (nextTrack != null)
            {
                groupNewTrack.DueTime =
                    DateTime.Now + TimeSpan.FromMilliseconds(nextTrack.SpotifyTrack.TrackDurationMs);
                _playbackScheduledTrackQueue.RemovePlaybackScheduledTrack(request.GroupId);
                _playbackScheduledTrackQueue.AddPlaybackScheduledTrack(groupNewTrack);

                await _spotifyPlaybackController.PlaySpotifyTrackForUsers(nextTrack);

                return PlaybackRequestResult.Success(new SkipTrackRequestResult(), "Track successfully skipped.");
            }

            return PlaybackRequestResult.Fail<SkipTrackRequestResult>("Couldn't get new tracks to play.");
        }
    }

    public class SkipTrackRequest : IPlaybackRequest<PlaybackRequestResult<SkipTrackRequestResult>>
    {
        public Guid GroupId { get; set; }
    }

    public class SkipTrackRequestResult
    {
    }
}