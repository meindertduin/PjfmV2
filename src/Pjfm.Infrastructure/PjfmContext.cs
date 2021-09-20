using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.ApplicationUser;
using Domain.SpotifyTrack;
using Domain.SpotifyUserData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pjfm.Infrastructure.TableMappings;

namespace Pjfm.Infrastructure
{
    public class PjfmContext : IdentityDbContext<ApplicationUser>, IPjfmContext
    {
        public DbSet<SpotifyTrack> SpotifyTracks { get; private set; } = null!;
        public DbSet<SpotifyUserData> SpotifyUserData { get; private set; } = null!;

        public PjfmContext(DbContextOptions<PjfmContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SpotifyTrackMap());
            builder.ApplyConfiguration(new SpotifyUserDataMap());
            builder.ApplyConfiguration(new ApplicationUserMap());

            base.OnModelCreating(builder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        public IEnumerable<EntityEntry<Entity>> ChangedEntries => ChangeTracker.Entries<Entity>();
    }
}