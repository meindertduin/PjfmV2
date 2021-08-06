using System.Threading.Tasks;
using Pjfm.Application.Spotify;
using Pjfm.Common;

namespace Pjfm.Application.Authentication
{
    /// <summary>
    /// This class functions as a wrapper for the GebruikerTokenService and functions kind of like a work around as
    /// GebruikerTokenService is a singleton service and can't consume the SpotifyAuthenticationService through DI.
    /// That's why this class is introduced.
    /// </summary>
    public class SpotifyTokenService : ISpotifyTokenService
    {
        private readonly IGebruikerTokenService _gebruikerTokenService;
        private readonly ISpotifyAuthenticationService _spotifyAuthenticationService;

        public SpotifyTokenService(IGebruikerTokenService gebruikerTokenService, ISpotifyAuthenticationService spotifyAuthenticationService)
        {
            _gebruikerTokenService = gebruikerTokenService;
            _spotifyAuthenticationService = spotifyAuthenticationService;
        }
        
        public async Task<GetAccessTokenResult> GetGebruikerSpotifyAccessToken(string gebruikerId)
        {
            Guard.NotNullOrEmpty(gebruikerId, nameof(gebruikerId));
            var gotAccessToken = _gebruikerTokenService.GetGebruikerSpotifyAccessToken(gebruikerId, out var spotifyAccessToken);

            if (!gotAccessToken)
            {
                var refreshResponse = await _spotifyAuthenticationService.RefreshAccessToken(gebruikerId);
                if (refreshResponse.IsSuccessful)
                {
                    _gebruikerTokenService.StoreGebruikerSpotifyAccessToken(gebruikerId, refreshResponse.Result.AccessToken, refreshResponse.Result.ExpiresIn);
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
        Task<GetAccessTokenResult> GetGebruikerSpotifyAccessToken(string gebruikerId);
    }
    
    public class GetAccessTokenResult
    {
        public bool IsSuccessful { get; set; }
        public string AccessToken { get; set; }
    }
}