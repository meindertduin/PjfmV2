using System.Threading.Tasks;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackRequestDispatcher
    {
        Task HandlePlaybackRequest<TRequest>(TRequest request) where TRequest : IPlaybackRequest;
        Task<TResult> HandlePlaybackRequest<TResult>(IPlaybackRequest<TResult> request);
    }
}