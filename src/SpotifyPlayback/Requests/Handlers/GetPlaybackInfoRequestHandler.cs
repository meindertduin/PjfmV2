using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class GetPlaybackInfoRequestHandler : IPlaybackRequestHandler<GetPlaybackInfoRequest, GetPlaybackInfoRequestResult>
    {
        private readonly IPlaybackGroepCollection _playbackGroepCollection;

        public GetPlaybackInfoRequestHandler(IPlaybackGroepCollection playbackGroepCollection)
        {
            _playbackGroepCollection = playbackGroepCollection;
        }
        public Task<GetPlaybackInfoRequestResult> HandleAsync(GetPlaybackInfoRequest request)
        {
            return Task.FromResult(new GetPlaybackInfoRequestResult()
            {
                PlaybackGroups = _playbackGroepCollection.GetPlaybackGroupsInfo(),
            });
        }
    }

    public class GetPlaybackInfoRequest : IPlaybackRequest<GetPlaybackInfoRequestResult>
    {
        
    }

    public class GetPlaybackInfoRequestResult
    {
        public IEnumerable<PlaybackGroupDto> PlaybackGroups { get; set; } = null!;
    }
}