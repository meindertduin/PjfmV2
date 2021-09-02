using System.Threading.Tasks;

namespace Domain.SpotifyGebruikerData
{
    public interface ISpotifyGebruikersDataRepository
    {
        Task<SpotifyUserData> GetSpotifyUserData(string userId);
        Task<SpotifyUserData> GetSpotifyUserDataAsNoTracking(string userId);
        Task<string> GetUserRefreshToken(string userId);
        Task SetUserRefreshToken(string userId, string refreshToken);
    }
}