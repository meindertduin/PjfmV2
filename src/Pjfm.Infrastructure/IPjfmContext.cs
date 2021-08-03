using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.SpotifyNummer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Pjfm.Infrastructure
{
    public interface IPjfmContext
    {
        DbSet<SpotifyNummer> SpotifyNummers { get; }
        Task<int> SaveChangesAsync();
        IEnumerable<EntityEntry<Entity>> ChangedEntries { get; }
    }
}