using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pjfm.Application.Authentication;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class SpotifyPlaybackService : ISpotifyPlaybackService
    {
        private readonly ISpotifyTokenService _spotifyTokenService;
        private readonly ISpotifyPlaybackClient _spotifyPlaybackClient;

        public SpotifyPlaybackService(ISpotifyTokenService spotifyTokenService, ISpotifyPlaybackClient spotifyPlaybackClient)
        {
            _spotifyTokenService = spotifyTokenService;
            _spotifyPlaybackClient = spotifyPlaybackClient;
        }

        public async Task PlayNextTrackForUsers(ListenerDto[] listeners, string trackId)
        {
            var playRequest = new SpotifyPlayRequestDto()
            {
                Uris = new[] {$"spotify:track:{trackId}"}
            };

            var getListenerAccessTokenTasks = (
                from listener in listeners
                where listener.Principal.IsAuthenticated()
                select Task.Run(async () =>
                {
                    var listenerAccessToken = await _spotifyTokenService.GetUserSpotifyAccessToken(listener.Principal.Id);
                    return (listenerAccessToken, listener);
                })).ToList();

            var listenerAccessTokenTuples = await Task.WhenAll(getListenerAccessTokenTasks);

            var playRequestTasks = new List<Task<bool>>();
            foreach (var listenerAccessTokenTuple in listenerAccessTokenTuples)
            {
                var accessToken = listenerAccessTokenTuple.listenerAccessToken.AccessToken;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var listenerDeviceId = listenerAccessTokenTuple.listener.DeviceId;
                    playRequestTasks.Add(_spotifyPlaybackClient.PlayTrackForUser(accessToken, playRequest, listenerDeviceId));
                }
            }

            await Task.WhenAll(playRequestTasks);
        }
    }
}