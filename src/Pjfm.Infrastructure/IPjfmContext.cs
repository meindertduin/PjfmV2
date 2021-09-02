using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.SpotifyGebruikerData;
using Domain.SpotifyTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Pjfm.Infrastructure
{
    public interface IPjfmContext
    {
        DbSet<SpotifyTrack> SpotifyNummers { get; }
        DbSet<SpotifyUserData> SpotifyGebruikerData { get; }
        Task<int> SaveChangesAsync();
        IEnumerable<EntityEntry<Entity>> ChangedEntries { get; }
    }
}