using System.Threading.Tasks;
using Pjfm.Application.Spotify;
using Pjfm.Common;

namespace Pjfm.Application.Authentication
{
    /// <summary>
    /// This class functions as a wrapper for the UserTokenService and functions kind of like a work around as
    /// UserTokenService is a singleton service and can't consume the SpotifyAuthenticationService through DI.
    /// That's why this class is introduced.
    /// </summary>
    public class SpotifyTokenService : ISpotifyTokenService
    {
        private readonly IUserTokenService _userTokenService;
        private readonly ISpotifyAuthenticationService _spotifyAuthenticationService;

        public SpotifyTokenService(IUserTokenService userTokenService, ISpotifyAuthenticationService spotifyAuthenticationService)
        {
            _userTokenService = userTokenService;
            _spotifyAuthenticationService = spotifyAuthenticationService;
        }
        
        public async Task<GetAccessTokenResult> GetUserSpotifyAccessToken(string userId)
        {
            Guard.NotNullOrEmpty(userId, nameof(userId));
            var gotAccessToken = _userTokenService.GetUserSpotifyAccessToken(userId, out var spotifyAccessToken);

            if (!gotAccessToken)
            {
                var refreshResponse = await _spotifyAuthenticationService.RefreshAccessToken(userId);
                if (refreshResponse.IsSuccessful)
                {
                    _userTokenService.StoreUserSpotifyAccessToken(userId, refreshResponse.Result.AccessToken, refreshResponse.Result.ExpiresIn);
                }
                else
                {
                    return new GetAccessTokenResult()
                    {
                        IsSuccessful = false,
                        AccessToken = string.Empty,
                    };
                }
            }

            return new GetAccessTokenResult()
            {
                IsSuccessful = true,
                AccessToken = spotifyAccessToken,
            };
        }
    }

    public interface ISpotifyTokenService
    {
        Task<GetAccessTokenResult> GetUserSpotifyAccessToken(string userId);
    }
    
    public class GetAccessTokenResult
    {
        public bool IsSuccessful { get; set; }
        public string AccessToken { get; set; }
    }
}