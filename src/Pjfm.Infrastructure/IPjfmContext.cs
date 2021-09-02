using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.SpotifyTrack;
using Domain.SpotifyUserData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Pjfm.Infrastructure
{
    public interface IPjfmContext
    {
        DbSet<SpotifyTrack> SpotifyTracks { get; }
        DbSet<SpotifyUserData> SpotifyUserData { get; }
        Task<int> SaveChangesAsync();
        IEnumerable<EntityEntry<Entity>> ChangedEntries { get; }
    }
}