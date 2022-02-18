using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class SkipTrackRequestHandler : IPlaybackRequestHandler<SkipTrackRequest, PlaybackRequestResult<SkipTrackRequestResult>>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public SkipTrackRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        
        public Task<PlaybackRequestResult<SkipTrackRequestResult>> HandleAsync(SkipTrackRequest request)
        {
            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(request.GroupId);
            playbackGroup.GetPlaybackQueue().SkipNextTrack();

            return Task.FromResult(PlaybackRequestResult.Success(new SkipTrackRequestResult(), "Track successfully skipped."));
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