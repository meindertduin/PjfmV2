using System;
using Domain.SpotifyTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class SpotifyTrackMap : IEntityTypeConfiguration<SpotifyTrack>
    {
        public void Configure(EntityTypeBuilder<SpotifyTrack> builder)
        {
            builder.ToTable("SpotifyTrack");
            builder.HasKey(s => s.Id);

            builder.HasIndex(s => s.UserId);

            builder.Property(s => s.SpotifyTrackId)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(s => s.UserId)
                .HasMaxLength(50);
            
            builder.Property(s => s.Artists).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(s => s.ApplicationUser)
                .WithMany(s => s.SpotifyTracks)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(s => s.SpotifyAlbum)
                .WithOne(s => s.SpotifyTrack)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
                
            builder.Property(s => s.Title)
                .HasMaxLength(256)
                .IsRequired();
            
            // TODO: fix the valueComparer issue
            // var artistsValueComparer = new ValueComparer<IEnumerable<string>>(
            //     (c1, c2) => c1.SequenceEqual(c2),
            //     c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //     c => c.ToList()
            // );
            //
            // builder.Property(s => s.Artists).Metadata.SetValueComparer(artistsValueComparer);
        }
    }
}