using EFCore.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.Task;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(p => p.CategoryId);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);
        builder.HasIndex(c => c.Name)
            .IsUnique();

        // HasData values must be constants — never DateTime.Now, or every
        // migration will think the data changed.
        var created = new DateTime(2025, 1, 1);

        builder.HasData(
            new Category { Id = 1, Name = "Electronics",    Description = "Devices and gadgets",      IsActive = true,  CreatedAt = created, UpdatedAt = created },
            new Category { Id = 2, Name = "Clothing",       Description = "Men and women apparel",    IsActive = true,  CreatedAt = created, UpdatedAt = created },
            new Category { Id = 3, Name = "Books",          Description = null,                       IsActive = true,  CreatedAt = created, UpdatedAt = created },
            new Category { Id = 4, Name = "Home & Kitchen", Description = "Appliances and cookware",  IsActive = true,  CreatedAt = created, UpdatedAt = created },
            new Category { Id = 5, Name = "Sports",         Description = "Fitness and outdoor gear", IsActive = true,  CreatedAt = created, UpdatedAt = created },
            new Category { Id = 6, Name = "Toys",           Description = "Discontinued category",    IsActive = false, CreatedAt = created, UpdatedAt = new DateTime(2025, 6, 1) }
        );
    }
}
