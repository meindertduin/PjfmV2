using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Pjfm.Application.Authentication
{
    public class GebruikerTokenService : IGebruikerTokenService
    {
        private ConcurrentDictionary<string, GebruikerTokensData> _gebruikerTokensData = new();

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
    }

    internal class GebruikerTokensData
    {
        // When adding new tokens, values such as SpotifyAccessToken might be set as nullable
        public string SpotifyAccessToken { get; set; } = null!;
        public DateTime SpotifyAccessTokenValidUntil { get; set; }
    }
}