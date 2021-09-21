using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyUserData;
using Microsoft.EntityFrameworkCore;

namespace Pjfm.Infrastructure.Repositories
{
    public class SpotifyUserDataRepository : ISpotifyUserDataRepository
    {
        private readonly IPjfmContext _pjfmContext;

        public SpotifyUserDataRepository(PjfmContext pjfmContext)
        {
            _pjfmContext = pjfmContext;
        }
        
        public Task<SpotifyUserData> GetSpotifyUserData(string userId)
        {
            return GetSpotifyUserDataQuery(userId).FirstOrDefaultAsync();
        }
        
        public Task<SpotifyUserData> GetSpotifyUserDataAsNoTracking(string userId)
        {
            return GetSpotifyUserDataQuery(userId).AsNoTracking().FirstOrDefaultAsync();
        }

        public Task<string> GetUserRefreshToken(string userId)
        {
            return _pjfmContext.SpotifyUserData
                .Where(s => s.UserId == userId)
                .Select(s => s.RefreshToken)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public Task RemoveUserSpotifyData(string userId)
        {
            var spotifyUserData = _pjfmContext.SpotifyUserData.FirstOrDefault(x => x.UserId == userId);
            if (spotifyUserData == null)
            {
                return Task.CompletedTask;
            }
            
            _pjfmContext.SpotifyUserData.Remove(spotifyUserData);
            return _pjfmContext.SaveChangesAsync();
        }

        public async Task SetUserRefreshToken(string userId, string refreshToken)
        {
            var spotifyUserData = await GetSpotifyUserData(userId);

            if (spotifyUserData != null)
            {
                spotifyUserData.RefreshToken = refreshToken;
            }
            else
            {
                _pjfmContext.SpotifyUserData.Add(new SpotifyUserData()
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                });
            }

            await _pjfmContext.SaveChangesAsync();
        }

        private IQueryable<SpotifyUserData> GetSpotifyUserDataQuery(string userId)
        {
            return _pjfmContext.SpotifyUserData.Where(x => x.UserId == userId);
        }

    }
}