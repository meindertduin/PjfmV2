using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Pjfm.Application.Spotify;
using Pjfm.Common;

namespace Pjfm.Application.Authentication
{
    public class GebruikerTokenService : IGebruikerTokenService
    {
        private readonly ISpotifyAuthenticationService _spotifyAuthenticationService;
        private ConcurrentDictionary<string, GebruikerTokensData> _gebruikerTokensData = new();

        public GebruikerTokenService(ISpotifyAuthenticationService spotifyAuthenticationService)
        {
            _spotifyAuthenticationService = spotifyAuthenticationService;
        }
        
        public void StoreGebruikerSpotifyAccessToken(string gebruikerId, string accessToken, int expiresIn)
        {
            if (_gebruikerTokensData.ContainsKey(gebruikerId))
            {
                _gebruikerTokensData.TryRemove(gebruikerId, out var gebruikerTokensData);

                gebruikerTokensData!.SpotifyAccessToken = accessToken;
                gebruikerTokensData.SpotifyAccessTokenValidUntil = DateTime.Now + new TimeSpan(0, 0, expiresIn);

                _gebruikerTokensData.TryAdd(gebruikerId, gebruikerTokensData);
            }
            else
            {
                _gebruikerTokensData.TryAdd(gebruikerId, new GebruikerTokensData()
                {
                    SpotifyAccessToken = accessToken,
                    SpotifyAccessTokenValidUntil = DateTime.Now + new TimeSpan(0, 0, expiresIn),
                });
            }
        }

        public async Task<GetAccessTokenResult> GetGebruikerSpotifyAccessToken(string gebruikerId)
        {
            Guard.NotNullOrEmpty(gebruikerId, nameof(gebruikerId));
            var gotAccessToken = GetGebruikerSpotifyAccessToken(gebruikerId, out var spotifyAccessToken);

            if (!gotAccessToken)
            {
                var refreshResponse = await _spotifyAuthenticationService.RefreshAccessToken(gebruikerId);
                if (refreshResponse.IsSuccessful)
                {
                    StoreGebruikerSpotifyAccessToken(gebruikerId, refreshResponse.Result.AccessToken, refreshResponse.Result.ExpiresIn);
                }
                else
                {
                    return new()
                    {
                        IsSuccessful = false,
                        AccessToken = string.Empty,
                    };
                }
            }

            return new ()
            {
                IsSuccessful = true,
                AccessToken = spotifyAccessToken,
            };
        }
        
        public bool GetGebruikerSpotifyAccessToken(string gebruikerId, [MaybeNullWhen(false)] out string accessToken)
        {
            var hasGebruikersData = _gebruikerTokensData.TryGetValue(gebruikerId, out var gebruikerTokensData);
            accessToken = null;

            if (!hasGebruikersData)
            {
                return false;
            }

            accessToken = gebruikerTokensData!.SpotifyAccessToken;
            
            if (gebruikerTokensData.SpotifyAccessTokenValidUntil > DateTime.Today)
            {
                return true;
            }

            return false;
        }
    }

    public interface IGebruikerTokenService
    {
        void StoreGebruikerSpotifyAccessToken(string gebruikerId, string accessToken, int expiresIn);
        bool GetGebruikerSpotifyAccessToken(string gebruikerId, [MaybeNullWhen(false)] out string accessToken);
        Task<GetAccessTokenResult> GetGebruikerSpotifyAccessToken(string gebruikerId);
    }

    internal class GebruikerTokensData
    {
        // When adding new tokens, values such as SpotifyAccessToken might be set as nullable
        public string SpotifyAccessToken { get; set; } = null!;
        public DateTime SpotifyAccessTokenValidUntil { get; set; }
    }

    public class GetAccessTokenResult
    {
        public bool IsSuccessful { get; set; }
        public string AccessToken { get; set; }
    }
}