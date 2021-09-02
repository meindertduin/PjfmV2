using System.Threading.Tasks;

namespace Domain.SpotifyUserData
{
    public interface ISpotifyUserDataRepository
    {
        Task<SpotifyUserData> GetSpotifyUserData(string userId);
        Task<SpotifyUserData> GetSpotifyUserDataAsNoTracking(string userId);
        Task<string> GetUserRefreshToken(string userId);
        Task SetUserRefreshToken(string userId, string refreshToken);
    }
}