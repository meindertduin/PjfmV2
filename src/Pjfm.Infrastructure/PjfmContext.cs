using Domain.Gebruiker;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pjfm.Infrastructure.TableMappings;

namespace Pjfm.Infrastructure
{
    public class PjfmContext : IdentityDbContext
    {
        public DbSet<Gebruiker> Gebruikers { get; private set; }
        
        public PjfmContext(DbContextOptions<PjfmContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GebruikerMap());
            
            base.OnModelCreating(builder);
        }
    }
}