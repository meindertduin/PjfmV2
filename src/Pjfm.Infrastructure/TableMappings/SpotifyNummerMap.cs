using System;
using System.Collections.Generic;
using System.Linq;
using Domain.SpotifyNummer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class SpotifyNummerMap : IEntityTypeConfiguration<SpotifyNummer>
    {
        public void Configure(EntityTypeBuilder<SpotifyNummer> builder)
        {
            builder.ToTable("SpotifyNummer");
            builder.HasKey(s => s.Id);

            builder.HasIndex(s => s.GebruikerId);

            builder.Property(s => s.SpotifyNummerId).HasMaxLength(50).IsRequired();
            builder.Property(s => s.GebruikerId).HasMaxLength(50);
            builder.Property(s => s.Artists).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .HasMaxLength(255)
                .IsRequired();

            var artistsValueComparer = new ValueComparer<IEnumerable<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToArray()
            );
            
            builder.Property(s => s.Artists).Metadata.SetValueComparer(artistsValueComparer);
            
            builder.Property(s => s.Titel).HasMaxLength(255).IsRequired();
        }
    }
}