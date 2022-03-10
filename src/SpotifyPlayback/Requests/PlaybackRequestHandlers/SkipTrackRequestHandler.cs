using System;
using System.Linq;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

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
            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(request.GroupId);

            var groupTrack = await playbackGroup.SkipTrack();

            var groupNewTrack = new PlaybackScheduledTrack()
            {
                SpotifyTrack = groupTrack,
                GroupId = request.GroupId,
            };

            groupNewTrack.DueTime =
                DateTime.Now + TimeSpan.FromMilliseconds(groupNewTrack.SpotifyTrack.TrackDurationMs);
            _playbackScheduledTrackQueue.RemovePlaybackScheduledTrack(request.GroupId);
            _playbackScheduledTrackQueue.AddPlaybackScheduledTrack(groupNewTrack);

            await _spotifyPlaybackController.PlaySpotifyTrackForUsers(groupNewTrack);

            return PlaybackRequestResult.Success(new SkipTrackRequestResult(), "Track successfully skipped.");
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