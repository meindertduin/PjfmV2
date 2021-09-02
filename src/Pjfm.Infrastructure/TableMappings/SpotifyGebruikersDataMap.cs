using Domain.SpotifyGebruikerData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class SpotifyGebruikersDataMap : IEntityTypeConfiguration<SpotifyUserData>
    {
        public void Configure(EntityTypeBuilder<SpotifyUserData> builder)
        {
            builder.ToTable("SpotifyGebruikersData");
            builder.HasKey(s => s.Id);
            builder.HasIndex(s => s.UserId);

            builder.Property(s => s.UserId).HasMaxLength(50).IsRequired();
            builder.Property(s => s.RefreshToken).IsRequired();
        }
    }
}