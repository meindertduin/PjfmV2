
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyGebruikerData;
using Microsoft.EntityFrameworkCore;

namespace Pjfm.Infrastructure.Repositories
{
    public class SpotifyGebruikerDataRepository : ISpotifyGebruikersDataRepository
    {
        private readonly IPjfmContext _pjfmContext;

        public SpotifyGebruikerDataRepository(PjfmContext pjfmContext)
        {
            _pjfmContext = pjfmContext;
        }
        
        public Task<SpotifyUserData> GetSpotifyUserData(string userId)
        {
            return GetSpotifyGebruikersDataQuery(userId).FirstOrDefaultAsync();
        }
        
        public Task<SpotifyUserData> GetSpotifyUserDataAsNoTracking(string userId)
        {
            return GetSpotifyGebruikersDataQuery(userId).AsNoTracking().FirstOrDefaultAsync();
        }

        public Task<string> GetUserRefreshToken(string userId)
        {
            return _pjfmContext.SpotifyGebruikerData
                .Where(s => s.UserId == userId)
                .Select(s => s.RefreshToken)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task SetUserRefreshToken(string userId, string refreshToken)
        {
            var spotifyGebruikerData = await GetSpotifyUserData(userId);

            if (spotifyGebruikerData != null)
            {
                spotifyGebruikerData.RefreshToken = refreshToken;
            }
            else
            {
                _pjfmContext.SpotifyGebruikerData.Add(new SpotifyUserData()
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                });
            }

            await _pjfmContext.SaveChangesAsync();
        }

        private IQueryable<SpotifyUserData> GetSpotifyGebruikersDataQuery(string gebruikerId)
        {
            return _pjfmContext.SpotifyGebruikerData.Where(x => x.UserId == gebruikerId);
        }

    }
}