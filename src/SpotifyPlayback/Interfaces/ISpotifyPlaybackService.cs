using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Interfaces
{
    public interface ISpotifyPlaybackService
    {
        Task PlayNextTrackForUsers(ListenerDto[] listeners, string trackId);
        Task PlayTrackForUser(ListenerDto listener, string trackId, int trackStartTimeMs);
        Task PausePlaybackForUser(string userId);
        Task<IEnumerable<DeviceDto>> GetUserDevices(string userId);
    }
}