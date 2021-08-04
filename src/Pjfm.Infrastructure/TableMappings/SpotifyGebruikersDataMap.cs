using Domain.SpotifyGebruikerData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class SpotifyGebruikersDataMap : IEntityTypeConfiguration<SpotifyGebruikerData>
    {
        public void Configure(EntityTypeBuilder<SpotifyGebruikerData> builder)
        {
            builder.ToTable("SpotifyGebruikersData");
            builder.HasKey(s => s.Id);
            builder.HasIndex(s => s.GebruikerId);

            builder.Property(s => s.GebruikerId).HasMaxLength(50).IsRequired();
            builder.Property(s => s.RefreshToken).IsRequired();
        }
    }
}