using System.Threading.Tasks;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Interfaces
{
    public interface ISpotifyPlaybackService
    {
        Task PlayNextTrackForUsers(ListenerDto[] listeners, string trackId);
        Task PlayTrackForUser(ListenerDto listener, string trackId);
    }
}