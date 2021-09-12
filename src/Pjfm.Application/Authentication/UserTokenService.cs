using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Pjfm.Application.Authentication
{
    public class UserTokenService : IUserTokenService
    {
        private ConcurrentDictionary<string, UserTokensData> _userTokensData = new();

        public void StoreUserSpotifyAccessToken(string userId, string accessToken, int expiresIn)
        {
            if (_userTokensData.ContainsKey(userId))
            {
                _userTokensData.TryRemove(userId, out var userTokensData);

                userTokensData!.SpotifyAccessToken = accessToken;
                userTokensData.SpotifyAccessTokenValidUntil = DateTime.Now + new TimeSpan(0, 0, expiresIn);

                _userTokensData.TryAdd(userId, userTokensData);
            }
            else
            {
                _userTokensData.TryAdd(userId, new UserTokensData()
                {
                    SpotifyAccessToken = accessToken,
                    SpotifyAccessTokenValidUntil = DateTime.Now + new TimeSpan(0, 0, expiresIn),
                });
            }
        }
        
        public bool GetUserSpotifyAccessToken(string userId, [MaybeNullWhen(false)] out string accessToken)
        {
            var hasUserData = _userTokensData.TryGetValue(userId, out var userTokensData);
            accessToken = null;

            if (!hasUserData)
            {
                return false;
            }

            accessToken = userTokensData!.SpotifyAccessToken;
            
            if (userTokensData.SpotifyAccessTokenValidUntil > DateTime.Today)
            {
                return true;
            }

            return false;
        }
    }

    public interface IUserTokenService
    {
        void StoreUserSpotifyAccessToken(string userId, string accessToken, int expiresIn);
        bool GetUserSpotifyAccessToken(string userId, [MaybeNullWhen(false)] out string accessToken);
    }

    internal class UserTokensData
    {
        // When adding new tokens, values such as SpotifyAccessToken might be set as nullable
        public string SpotifyAccessToken { get; set; } = null!;
        public DateTime SpotifyAccessTokenValidUntil { get; set; }
    }

}