using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class
        GetPlaybackGroupsRequestHandler : IPlaybackRequestHandler<GetPlaybackGroupsRequest,
            GetPlaybackGroepenRequestResult>
    {
        private readonly IPlaybackGroepCollection _playbackGroepCollection;

        public GetPlaybackGroupsRequestHandler(IPlaybackGroepCollection playbackGroepCollection)
        {
            _playbackGroepCollection = playbackGroepCollection;
        }

        public Task<GetPlaybackGroepenRequestResult> HandleAsync(GetPlaybackGroupsRequest request)
        {
            return Task.FromResult(new GetPlaybackGroepenRequestResult()

            {
                PlaybackGroups = _playbackGroepCollection.GetPlaybackGroupsInfo(),
            });
        }
    }

    public class GetPlaybackGroupsRequest : IPlaybackRequest<GetPlaybackGroepenRequestResult>
    {
    }

    public class GetPlaybackGroepenRequestResult
    {
        public IEnumerable<PlaybackGroupDto> PlaybackGroups { get; set; } = null!;
    }
}