using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.EntityFrameworkCore;

namespace Pjfm.Infrastructure.Repositories
{
    public class SpotifyTrackRepository : ISpotifyTrackRepository
    {
        private readonly IPjfmContext _pjfmContext;

        public SpotifyTrackRepository(PjfmContext pjfmContext)
        {
            _pjfmContext = pjfmContext;
        }

        public Task<List<SpotifyTrack>> GetUserSpotifyTracksByUserId(string userId)
        {
            return _pjfmContext.SpotifyTracks.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task SetUserSpotifyTracks(IEnumerable<SpotifyTrack> spotifyTracks, string userId)
        {
            var alreadyAvailableSpotifyTracks = await _pjfmContext.SpotifyTracks
                .Where(s => s.UserId == userId)
                .AsNoTracking()
                .ToArrayAsync();

            if (alreadyAvailableSpotifyTracks.Length > 0)
            {
                _pjfmContext.SpotifyTracks.RemoveRange(alreadyAvailableSpotifyTracks);
            }

            await _pjfmContext.SpotifyTracks.AddRangeAsync(spotifyTracks);
            await _pjfmContext.SaveChangesAsync();
        }

        public async Task RemoveUserSpotifyTracks(string userId)
        {
            var spotifyTracks = await _pjfmContext.SpotifyTracks
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            _pjfmContext.SpotifyTracks.RemoveRange(spotifyTracks);

            await _pjfmContext.SaveChangesAsync();
        }

        public Task<List<SpotifyTrack>> GetRandomUserSpotifyTracks(string[] userIds, IEnumerable<TrackTerm> terms,
            int amount)
        {
            var query = _pjfmContext.SpotifyTracks
                .Where(s => terms.Contains(s.TrackTerm));
            
            if (userIds.Any())
            {
                query = query.Where(s => userIds.Contains(s.UserId));
            }

            return query.Include(x => x.SpotifyAlbum)
                .ThenInclude(x => x.AlbumImage)
                .OrderBy(s => Guid.NewGuid())
                .Take(amount)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}