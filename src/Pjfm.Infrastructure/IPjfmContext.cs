using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.SpotifyGebruikerData;
using Domain.SpotifyNummer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Pjfm.Infrastructure
{
    public interface IPjfmContext
    {
        DbSet<SpotifyNummer> SpotifyNummers { get; }
        DbSet<SpotifyGebruikerData> SpotifyGebruikerData { get; }
        Task<int> SaveChangesAsync();
        IEnumerable<EntityEntry<Entity>> ChangedEntries { get; }
    }
}