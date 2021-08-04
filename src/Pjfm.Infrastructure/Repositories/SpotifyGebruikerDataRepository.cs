
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
        
        public Task<SpotifyGebruikerData> GetSpotifyGebruikerData(string gebruikerId)
        {
            return GetSpotifyGebruikersDataQuery(gebruikerId).FirstOrDefaultAsync();
        }
        
        public Task<SpotifyGebruikerData> GetSpotifyGebruikerDataAsNoTracking(string gebruikerId)
        {
            return GetSpotifyGebruikersDataQuery(gebruikerId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task SetGebruikerRefreshToken(string gebruikerId, string refreshToken)
        {
            var spotifyGebruikerData = await GetSpotifyGebruikerData(gebruikerId);

            if (spotifyGebruikerData != null)
            {
                spotifyGebruikerData.RefreshToken = refreshToken;
            }
            else
            {
                _pjfmContext.SpotifyGebruikerData.Add(new SpotifyGebruikerData()
                {
                    GebruikerId = gebruikerId,
                    RefreshToken = refreshToken,
                });
            }

            await _pjfmContext.SaveChangesAsync();
        }

        private IQueryable<SpotifyGebruikerData> GetSpotifyGebruikersDataQuery(string gebruikerId)
        {
            return _pjfmContext.SpotifyGebruikerData.Where(x => x.GebruikerId == gebruikerId);
        }

    }
}