using System.Threading.Tasks;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackRequestHandler<in TRequest> where TRequest : IPlaybackRequest
    {
        Task HandleAsync(TRequest request, SocketConnection socketConnection);
    }
    
    public interface IPlaybackRequestHandler<in TRequest, TResult> where TRequest : IPlaybackRequest<TResult>
    {
        Task<TResult> HandleAsync(TRequest request);
    }
}