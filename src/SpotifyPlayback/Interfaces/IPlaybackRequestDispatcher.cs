using System.Threading.Tasks;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackRequestDispatcher
    {
        Task HandlePlaybackRequest<TRequest>(TRequest request, SocketConnection connection) where TRequest : IPlaybackRequest;
        Task<TResult> HandlePlaybackRequest<TResult>(IPlaybackRequest<TResult> request);
    }
}