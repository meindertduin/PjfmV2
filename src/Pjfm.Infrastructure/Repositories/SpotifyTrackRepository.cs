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

        public async Task SetUserSpotifyTracks(IEnumerable<SpotifyTrack> spotifyTracks ,string userId)
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

        public Task<List<SpotifyTrack>> GetRandomUserSpotifyTracks(IEnumerable<string> userIds, IEnumerable<TrackTerm> terms, int amount)
        {
            return _pjfmContext.SpotifyTracks
                // TODO: add userIds
                // .Where(s => userIds.Contains(s.userId))
                .Where(s => terms.Contains(s.TrackTerm))
                .OrderBy(s => Guid.NewGuid())
                .Take(amount)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}