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

            builder.Property(s => s.ReleaseDate)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(s => s.Title)
                .HasMaxLength(255).IsRequired();

            builder.HasOne(x => x.AlbumImage)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(x => x.SpotifyTrack)
                .WithOne(x => x.SpotifyAlbum)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}