using Domain.Gebruiker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class GebruikerMap : IEntityTypeConfiguration<Gebruiker>
    {
        public void Configure(EntityTypeBuilder<Gebruiker> builder)
        {
            builder.ToTable("Gebruiker");
            builder.HasKey(c => c.Id);

            builder.Property(x => x.IdentityUserId).IsRequired();
            builder.HasIndex(x => x.IdentityUserId).IsUnique();

            builder.Property(x => x.GebruikersNaam).HasMaxLength(225).IsRequired(true);
        }
    }
}