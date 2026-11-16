using EFCore.Models.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S3;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.Price)
            .HasPrecision(10, 2);

        builder.Property(c => c.Level)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.PublishedDate)
            .IsRequired(false);

        builder.HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Category)
            .WithMany(cat => cat.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Course { Id = 1, Title = "ASP.NET Core Web API",    Description = "Build REST APIs with ASP.NET Core.", Price = 49.99m, Level = CourseLevel.Intermediate, IsPublished = true,  PublishedDate = new DateTime(2025, 1, 15), InstructorId = 1, CategoryId = 5 },
            new Course { Id = 2, Title = "C# Fundamentals",         Description = "Start programming with C#.",         Price = 19.99m, Level = CourseLevel.Beginner,     IsPublished = true,  PublishedDate = new DateTime(2024, 11, 1), InstructorId = 1, CategoryId = 3 },
            new Course { Id = 3, Title = "Advanced EF Core",        Description = null,                                 Price = 79.99m, Level = CourseLevel.Advanced,     IsPublished = false, PublishedDate = null,                      InstructorId = 2, CategoryId = 5 },
            new Course { Id = 4, Title = "React from Scratch",      Description = "Components, hooks and state.",       Price = 29.99m, Level = CourseLevel.Beginner,     IsPublished = true,  PublishedDate = new DateTime(2025, 3, 5),  InstructorId = 2, CategoryId = 4 },
            new Course { Id = 5, Title = "Modern CSS Layouts",      Description = "Flexbox and Grid in depth.",         Price = 24.99m, Level = CourseLevel.Intermediate, IsPublished = false, PublishedDate = null,                      InstructorId = 3, CategoryId = 4 },
            new Course { Id = 6, Title = "UI/UX Design Principles", Description = "Design usable interfaces.",          Price = 59.99m, Level = CourseLevel.Advanced,     IsPublished = true,  PublishedDate = new DateTime(2025, 5, 20), InstructorId = 3, CategoryId = 6 }
        );
    }
}
