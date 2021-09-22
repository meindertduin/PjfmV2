using Domain.SpotifyTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class SpotifyAlbumImageMap : IEntityTypeConfiguration<SpotifyAlbumImage>
    {
        public void Configure(EntityTypeBuilder<SpotifyAlbumImage> builder)
        {
            builder.ToTable(nameof(SpotifyAlbumImage));
            builder.HasKey(s => s.Id);

            builder.Property(x => x.Url)
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(x => x.Height)
                .IsRequired();
            
            builder.Property(x => x.Width)
                .IsRequired();
            
            builder.HasOne(s => s.SpotifyAlbum)
                .WithMany(s => s.AlbumImages)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}