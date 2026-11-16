using EFCore.Models.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S3;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable("Instructors");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Email)
            .IsRequired()
            .HasMaxLength(150);
        builder.HasIndex(i => i.Email).IsUnique();

        builder.Property(i => i.Bio)
            .HasMaxLength(500);

        builder.Property(i => i.JoinedAt)
            .HasColumnType("date");

        builder.HasData(
            new Instructor { Id = 1, FullName = "Ahmed Fathy",  Email = "ahmed.fathy@learn.com",  Bio = "Senior .NET developer, 10 years of backend work.", JoinedAt = new DateTime(2023, 1, 10) },
            new Instructor { Id = 2, FullName = "Sara Mahmoud", Email = "sara.mahmoud@learn.com", Bio = "Full-stack engineer and React trainer.",          JoinedAt = new DateTime(2023, 6, 1) },
            new Instructor { Id = 3, FullName = "Hassan Ali",   Email = "hassan.ali@learn.com",   Bio = null,                                               JoinedAt = new DateTime(2024, 2, 20) }
        );
    }
}
