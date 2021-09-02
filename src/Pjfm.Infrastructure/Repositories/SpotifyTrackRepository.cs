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
            return _pjfmContext.SpotifyNummers.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task SetUserSpotifyTracks(IEnumerable<SpotifyTrack> spotifyTracks ,string userId)
        {
            var alreadyAvailableSpotifyNummers = await _pjfmContext.SpotifyNummers
                .Where(s => s.UserId == userId)
                .AsNoTracking()
                .ToArrayAsync();

            if (alreadyAvailableSpotifyNummers.Length > 0)
            {
                _pjfmContext.SpotifyNummers.RemoveRange(alreadyAvailableSpotifyNummers);
            }

            await _pjfmContext.SpotifyNummers.AddRangeAsync(spotifyTracks);
            await _pjfmContext.SaveChangesAsync();
        }

        public Task<List<SpotifyTrack>> GetRandomUserSpotifyTracks(IEnumerable<string> userIds, IEnumerable<TrackTerm> terms, int amount)
        {
            return _pjfmContext.SpotifyNummers
                // TODO: add gebruikerIds
                // .Where(s => gebruikerIds.Contains(s.GebruikerId))
                .Where(s => terms.Contains(s.TrackTerm))
                .OrderBy(s => Guid.NewGuid())
                .Take(amount)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}