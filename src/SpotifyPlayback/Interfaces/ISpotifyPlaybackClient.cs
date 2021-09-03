using System.Net.Http;
using System.Threading.Tasks;

namespace SpotifyPlayback.Interfaces
{
    public interface ISpotifyPlaybackClient
    {
        Task<bool> PlayTrackForUser(string accessToken, string? deviceId = null);
        Task<bool> PausePlayer(string accessToken, string? deviceId = null);
    }
}