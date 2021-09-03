using Domain.SpotifyUserData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class SpotifyUserDataMap : IEntityTypeConfiguration<SpotifyUserData>
    {
        public void Configure(EntityTypeBuilder<SpotifyUserData> builder)
        {
            builder.ToTable("SpotifyUserData");
            builder.HasKey(s => s.Id);
            builder.HasIndex(s => s.UserId);

            builder.Property(s => s.UserId).HasMaxLength(50).IsRequired();
            builder.Property(s => s.RefreshToken).IsRequired();
        }
    }
}