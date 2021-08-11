using System.Threading.Tasks;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackRequestHandler<in TRequest> where TRequest : IPlaybackRequest
    {
        Task HandleAsync(TRequest request);
    }
    
    public interface IPlaybackRequestHandler<in TRequest, TResult> where TRequest : IPlaybackRequest<TResult>
    {
        Task<TResult> HandleAsync(TRequest request);
    }
}