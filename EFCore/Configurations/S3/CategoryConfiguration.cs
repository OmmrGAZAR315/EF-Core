using EFCore.Models.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S3;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Category { Id = 1, Name = "Programming", Description = "Software development courses", ParentCategoryId = null },
            new Category { Id = 2, Name = "Design",      Description = "Visual and product design",    ParentCategoryId = null },
            new Category { Id = 3, Name = "Backend",     Description = "Server-side development",      ParentCategoryId = 1 },
            new Category { Id = 4, Name = "Frontend",    Description = "Client-side development",      ParentCategoryId = 1 },
            new Category { Id = 5, Name = "ASP.NET",     Description = null,                           ParentCategoryId = 3 },
            new Category { Id = 6, Name = "UI/UX",       Description = "User interface & experience",  ParentCategoryId = 2 }
        );
    }
}
