using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class SpotifyPlaybackService : ISpotifyPlaybackService
    {
        private readonly ISpotifyPlaybackClient _spotifyPlaybackClient;

        public SpotifyPlaybackService(ISpotifyPlaybackClient spotifyPlaybackClient)
        {
            _spotifyPlaybackClient = spotifyPlaybackClient;
        }

        public async Task PlayNextTrackForUsers(ListenerDto[] listeners, string trackId)
        {
            var playRequest = new SpotifyPlayRequestDto()
            {
                Uris = new[] {$"spotify:track:{trackId}"}
            };

            var playRequestTasks = new List<Task<bool>>();
            foreach (var listener in listeners)
            {
                playRequestTasks.Add(_spotifyPlaybackClient.PlayTrackForUser(listener.Principal.Id, playRequest, listener.DeviceId));
            }

            await Task.WhenAll(playRequestTasks);
        }

        public Task PlayTrackForUser(ListenerDto listener, string trackId ,int trackStartTimeMs)
        {
            var playRequest = new SpotifyPlayRequestDto()
            {
                Uris = new[] {$"spotify:track:{trackId}"},
                PositionMs = trackStartTimeMs
            };

            return _spotifyPlaybackClient.PlayTrackForUser(listener.Principal.Id, playRequest, listener.DeviceId);
        }

        public async Task PausePlaybackForUser(string userId)
        {
            await _spotifyPlaybackClient.PausePlayer(userId);
        }

        public async Task<IEnumerable<DeviceDto>> GetUserDevices(string userId)
        {
            return await _spotifyPlaybackClient.GetPlaybackDevices(userId);
        }
    }
}