using Domain.ApplicationUser;
using Domain.SpotifyUserData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class ApplicationUserMap : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.Id).IsRequired().HasMaxLength(50);
            
            builder.HasMany(x => x.SpotifyTracks)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.SpotifyUserData)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey<SpotifyUserData>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(x => x.FillerQueueParticipantGroups)
                .WithMany(x => x.FillerQueueParticipants);
        }
    }
}