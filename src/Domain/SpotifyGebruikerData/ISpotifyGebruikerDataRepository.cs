using System.Threading.Tasks;

namespace Domain.SpotifyGebruikerData
{
    public interface ISpotifyGebruikersDataRepository
    {
        Task<SpotifyGebruikerData> GetSpotifyGebruikerData(string gebruikerId);
        Task<SpotifyGebruikerData> GetSpotifyGebruikerDataAsNoTracking(string gebruikerId);
        Task<string> GetGebruikerRefreshToken(string gebruikerId);
        Task SetGebruikerRefreshToken(string gebruikerId, string refreshToken);
    }
}