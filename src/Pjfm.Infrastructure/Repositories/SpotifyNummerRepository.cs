using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyNummer;
using Microsoft.EntityFrameworkCore;

namespace Pjfm.Infrastructure.Repositories
{
    public class SpotifyNummerRepository : ISpotifyNummerRepository
    {
        private readonly IPjfmContext _pjfmContext;

        public SpotifyNummerRepository(PjfmContext pjfmContext)
        {
            _pjfmContext = pjfmContext;
        }

        public Task<List<SpotifyNummer>> GetGebruikerSpotifyNummersByGebruikersId(string gebruikersId)
        {
            return _pjfmContext.SpotifyNummers.Where(x => x.GebruikerId == gebruikersId).ToListAsync();
        }

        public async Task SetGebruikerSpotifyNummers(IEnumerable<SpotifyNummer> spotifyNummers ,string gebruikerId)
        {
            var alreadyAvailableSpotifyNummers = await _pjfmContext.SpotifyNummers
                .Where(s => s.GebruikerId == gebruikerId)
                .AsNoTracking()
                .ToArrayAsync();

            if (alreadyAvailableSpotifyNummers.Length > 0)
            {
                _pjfmContext.SpotifyNummers.RemoveRange(alreadyAvailableSpotifyNummers);
            }

            await _pjfmContext.SpotifyNummers.AddRangeAsync(spotifyNummers);
            await _pjfmContext.SaveChangesAsync();
        }

        public Task<List<SpotifyNummer>> GetRandomGebruikersSpotifyNummers(IEnumerable<string> gebruikerIds, IEnumerable<TrackTermijn> termijnen, int amount)
        {
            return _pjfmContext.SpotifyNummers
                .Where(s => gebruikerIds.Contains(s.GebruikerId))
                .Where(s => termijnen.Contains(s.TrackTermijn))
                .OrderBy(s => Guid.NewGuid())
                .Take(amount)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}