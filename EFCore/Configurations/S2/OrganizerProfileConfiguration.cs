using EFCore.Models.S2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S2;

public class OrganizerProfileConfiguration : IEntityTypeConfiguration<OrganizerProfile>
{
    public void Configure(EntityTypeBuilder<OrganizerProfile> builder)
    {
        builder.HasOne(op => op.Organizer)
            .WithOne(o => o.Profile)
            .HasForeignKey<OrganizerProfile>(o => o.OrganizerId);
    }
}