using Domain.SpotifyTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class SpotifyAlbumMap : IEntityTypeConfiguration<SpotifyAlbum>
    {
        public void Configure(EntityTypeBuilder<SpotifyAlbum> builder)
        {
            builder.ToTable(nameof(SpotifyAlbum));
            builder.HasKey(s => s.Id);
            builder.Property(s => s.AlbumId)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(s => s.Title)
                .HasMaxLength(255).IsRequired();
            
            builder.HasMany(s => s.AlbumImages)
                .WithOne(s => s.SpotifyAlbum)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SpotifyTracks)
                .WithOne(x => x.SpotifyAlbum)
                .HasForeignKey(x => x.SpotifyAlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}