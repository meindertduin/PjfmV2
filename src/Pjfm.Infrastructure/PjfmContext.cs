using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.SpotifyGebruikerData;
using Domain.SpotifyTrack;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pjfm.Infrastructure.TableMappings;

namespace Pjfm.Infrastructure
{
    public class PjfmContext : IdentityDbContext, IPjfmContext
    {
        public DbSet<SpotifyTrack> SpotifyNummers { get; private set; } = null!;
        public DbSet<SpotifyUserData> SpotifyGebruikerData { get; private set; } = null!;

        public PjfmContext(DbContextOptions<PjfmContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SpotifyTrackMap());
            builder.ApplyConfiguration(new SpotifyGebruikersDataMap());
            
            base.OnModelCreating(builder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        public IEnumerable<EntityEntry<Entity>> ChangedEntries => ChangeTracker.Entries<Entity>();
    }
}