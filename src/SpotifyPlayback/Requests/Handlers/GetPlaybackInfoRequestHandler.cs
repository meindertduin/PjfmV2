using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.Handlers
{
    public class GetPlaybackInfoRequestHandler : IPlaybackRequestHandler<GetPlaybackInfoRequest, GetPlaybackRequestResult>
    {
        public Task<GetPlaybackRequestResult> HandleAsync(GetPlaybackInfoRequest request)
        {
            // TODO: data is rubbish for testing purpose
            return Task.FromResult(new GetPlaybackRequestResult()
            {
                Message = "Dit werkt dus..."
            });
        }
    }

    public class GetPlaybackInfoRequest : IPlaybackRequest<GetPlaybackRequestResult>
    {
        
    }

    public class GetPlaybackRequestResult
    {
        public string Message { get; set; }
    }
}