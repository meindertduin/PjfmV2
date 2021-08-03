namespace Pjfm.Application.Spotify
{
    public class SpotifyAccessTokenResult
    {
        public string SpotifyAccessToken { get; init; } = null!;
        public string SpotifyRefreshToken { get; init;  } = null!;
    }
}