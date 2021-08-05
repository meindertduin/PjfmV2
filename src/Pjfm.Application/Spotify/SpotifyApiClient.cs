using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pjfm.Application.Authentication;
using Pjfm.Common;

namespace Pjfm.Application.Spotify
{
    public class SpotifyApiClient : ISpotifyApiClient
    {
        private readonly IGebruikerTokenService _gebruikerTokenService;
        private readonly ISpotifyAuthenticationService _spotifyAuthenticationService;

        public SpotifyApiClient(IGebruikerTokenService gebruikerTokenService,
            ISpotifyAuthenticationService spotifyAuthenticationService)
        {
            _gebruikerTokenService = gebruikerTokenService;
            _spotifyAuthenticationService = spotifyAuthenticationService;
        }

        public async Task<T> MakeSpotifyClientRequest<T>(string gebruikerId, Func<string, T> spotifyClientRequest)
        {
            Guard.NotNullOrEmpty(gebruikerId, nameof(gebruikerId));
            var gotAccessToken =
                _gebruikerTokenService.GetGebruikerSpotifyAccessToken(gebruikerId, out var spotifyAccessToken);

            if (!gotAccessToken)
            {
                var refreshResponse = await _spotifyAuthenticationService.RefreshAccessToken(gebruikerId);
                if (refreshResponse.IsSuccessful)
                {
                    spotifyAccessToken = refreshResponse.Result.AccessToken;
                }
                else
                {
                    throw new HttpRequestException("Request failed");
                }
            }

            Guard.NotNullOrEmpty(spotifyAccessToken, nameof(spotifyAccessToken));
            return spotifyClientRequest.Invoke(spotifyAccessToken);
        }
    }

    public interface ISpotifyApiClient
    {
        Task<T> MakeSpotifyClientRequest<T>(string gebruikerId, Func<string, T> spotifyClientRequest);
    }
}