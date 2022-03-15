using Domain.SessionGroup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pjfm.Infrastructure.TableMappings
{
    public class SessionGroupMap : IEntityTypeConfiguration<SessionGroup>
    {
        public void Configure(EntityTypeBuilder<SessionGroup> builder)
        {
            builder.ToTable(nameof(SessionGroup));
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(x => x.GroupName)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasMany(x => x.FillerQueueParticipants)
                .WithMany(x => x.FillerQueueParticipantGroups);
        }
    }
}