using EFCore.Models.S2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S2;

public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
{
    public void Configure(EntityTypeBuilder<Badge> builder)
    {
        builder.HasOne(b => b.Attendee)
            .WithOne(a => a.Badge)
            .HasForeignKey<Badge>(b => b.AttendeeId);
        
        builder.HasIndex(b=>b.Number).IsUnique();
    }
}