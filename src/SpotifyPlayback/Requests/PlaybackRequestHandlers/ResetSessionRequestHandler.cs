using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class ResetSessionRequestHandler : IPlaybackRequestHandler<ResetSessionRequest, PlaybackRequestResult<ResetSessionRequestResult>>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly IPlaybackScheduledTrackQueue _playbackScheduledTrackQueue;
        private readonly ISpotifyPlaybackController _spotifyPlaybackController;

        public ResetSessionRequestHandler(IPlaybackGroupCollection playbackGroupCollection, IPlaybackScheduledTrackQueue playbackScheduledTrackQueue, ISpotifyPlaybackController spotifyPlaybackController)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _playbackScheduledTrackQueue = playbackScheduledTrackQueue;
            _spotifyPlaybackController = spotifyPlaybackController;
        }
        
        public async Task<PlaybackRequestResult<ResetSessionRequestResult>> HandleAsync(ResetSessionRequest request)
        {
            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(request.GroupId);
            playbackGroup.ResetTracks();

            await playbackGroup.SkipTrack();
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

            return PlaybackRequestResult.Success(new ResetSessionRequestResult(), "Track successfully skipped.");
        }
    }
    
    public class ResetSessionRequest : IPlaybackRequest<PlaybackRequestResult<ResetSessionRequestResult>>
    {
        public string GroupId { get; set; }
    }

    public class ResetSessionRequestResult
    {
    }
}