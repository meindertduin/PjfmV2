using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Interfaces
{
    public interface ISpotifyPlaybackClient
    {
        Task<bool> PlayTrackForUser(string accessToken, SpotifyPlayRequestDto content, string deviceId);
        Task<bool> PausePlayer(string accessToken, string? deviceId = null);
        Task<IEnumerable<DeviceDto>> GetPlaybackDevices(string accessToken);
    }
}