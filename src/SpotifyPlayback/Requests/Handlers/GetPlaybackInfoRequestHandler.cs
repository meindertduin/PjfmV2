using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class GetPlaybackInfoRequestHandler : IPlaybackRequestHandler<GetPlaybackInfoRequest, GetPlaybackInfoRequestResult>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public GetPlaybackInfoRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        public Task<GetPlaybackInfoRequestResult> HandleAsync(GetPlaybackInfoRequest request)
        {
            return Task.FromResult(new GetPlaybackInfoRequestResult()
            {
                PlaybackGroups = _playbackGroupCollection.getPlaybackGroupsInfo(),
            });
        }
    }

    public class GetPlaybackInfoRequest : IPlaybackRequest<GetPlaybackInfoRequestResult>
    {
        
    }

    public class GetPlaybackInfoRequestResult
    {
        public IEnumerable<PlaybackGroupDto> PlaybackGroups { get; set; }
    }
}