using System.Threading.Tasks;

namespace SpotifyPlayback.Interfaces
{
    public interface ISpotifyPlaybackController
    {
        Task PlaySpotifyTrackForUsers(string[] userIds);
        Task PauseSpotifyPlayerUser();
    }
}