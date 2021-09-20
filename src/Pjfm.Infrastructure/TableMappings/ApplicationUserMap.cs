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
            builder.HasMany(x => x.SpotifyTracks)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(x => x.SpotifyUserData)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey<SpotifyUserData>(x => x.UserId);
        }
    }
}