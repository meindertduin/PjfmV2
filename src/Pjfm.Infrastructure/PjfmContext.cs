using Domain.SpotifyTrack;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pjfm.Infrastructure.TableMappings;

namespace Pjfm.Infrastructure
{
    public class PjfmContext : IdentityDbContext
    {
        public DbSet<SpotifyNummer> SpotifyNummers { get; private set; } = null!;
        
        public PjfmContext(DbContextOptions<PjfmContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SpotifyNummerMap());
            
            base.OnModelCreating(builder);
        }
    }
}