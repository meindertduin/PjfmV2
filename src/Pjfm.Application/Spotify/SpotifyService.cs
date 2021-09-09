using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pjfm.Application.Authentication;
using SpotifyAPI.Web;

namespace Pjfm.Application.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        private readonly ISpotifyTokenService _spotifyTokenService;

        public SpotifyService(ISpotifyTokenService spotifyTokenService)
        {
            _spotifyTokenService = spotifyTokenService;
        }
        
        public async Task<IEnumerable<Device>> GetUserDevices(string userId)
        {
            var accessTokenResult = await _spotifyTokenService.GetUserSpotifyAccessToken(userId);
            if (accessTokenResult.IsSuccessful)
            {
                var spotifyClient = new SpotifyClient(accessTokenResult.AccessToken);
                var availableDevices = await spotifyClient.Player.GetAvailableDevices();
                return availableDevices.Devices;
            }
            
            return Enumerable.Empty<Device>();
        }
    }

    public interface ISpotifyService
    {
        public Task<IEnumerable<Device>> GetUserDevices(string userId);
    }
}