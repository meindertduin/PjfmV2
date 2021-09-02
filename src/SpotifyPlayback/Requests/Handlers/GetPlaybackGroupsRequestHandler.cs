using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class
        GetPlaybackGroupsRequestHandler : IPlaybackRequestHandler<GetPlaybackGroupsRequest,
            GetPlaybackGroupsRequestResult>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public GetPlaybackGroupsRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }

        public Task<GetPlaybackGroupsRequestResult> HandleAsync(GetPlaybackGroupsRequest request)
        {
            return Task.FromResult(new GetPlaybackGroupsRequestResult()

            {
                PlaybackGroups = _playbackGroupCollection.GetPlaybackGroupsInfo(),
            });
        }
    }

    public class GetPlaybackGroupsRequest : IPlaybackRequest<GetPlaybackGroupsRequestResult>
    {
    }

    public class GetPlaybackGroupsRequestResult
    {
        public IEnumerable<PlaybackGroupDto> PlaybackGroups { get; set; } = null!;
    }
}