using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.Handlers
{
    public class JoinPlaybackGroupRequestHandler : IPlaybackRequestHandler<JoinPlaybackGroupRequest, JoinPlaybackGroupResult>
    {
        public Task<JoinPlaybackGroupResult> HandleAsync(JoinPlaybackGroupRequest request)
        {
            
        }
    }

    public class JoinPlaybackGroupRequest : IPlaybackRequest<JoinPlaybackGroupResult>
    {
        
    }

    public class JoinPlaybackGroupResult
    {
        
    }
}