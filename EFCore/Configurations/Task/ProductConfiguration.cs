using EFCore.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.Task;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(p => p.CategoryId);

        builder.Property(p => p.Price).HasPrecision(10, 2);
        builder.HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(oi => oi.ProductId);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            // Electronics
            new Product { Id = 1,  Name = "Laptop",              Price = 999.99m, Quantity = 10, IsAvailable = true,  CategoryId = 1 },
            new Product { Id = 2,  Name = "Smartphone",          Price = 499.99m, Quantity = 20, IsAvailable = true,  CategoryId = 1 },
            new Product { Id = 3,  Name = "Wireless Headphones", Price = 79.99m,  Quantity = 0,  IsAvailable = false, CategoryId = 1 },
            // Clothing
            new Product { Id = 4,  Name = "T-Shirt",             Price = 19.99m,  Quantity = 50, IsAvailable = true,  CategoryId = 2 },
            new Product { Id = 5,  Name = "Jeans",               Price = 39.99m,  Quantity = 30, IsAvailable = true,  CategoryId = 2 },
            new Product { Id = 6,  Name = "Winter Jacket",       Price = 129.99m, Quantity = 5,  IsAvailable = true,  CategoryId = 2 },
            // Books
            new Product { Id = 7,  Name = "Novel",               Price = 14.99m,  Quantity = 40, IsAvailable = true,  CategoryId = 3 },
            new Product { Id = 8,  Name = "Cookbook",            Price = 24.99m,  Quantity = 15, IsAvailable = true,  CategoryId = 3 },
            // Home & Kitchen
            new Product { Id = 9,  Name = "Coffee Maker",        Price = 89.99m,  Quantity = 8,  IsAvailable = true,  CategoryId = 4 },
            new Product { Id = 10, Name = "Blender",             Price = 59.99m,  Quantity = 0,  IsAvailable = false, CategoryId = 4 },
            // Sports
            new Product { Id = 11, Name = "Yoga Mat",            Price = 29.99m,  Quantity = 25, IsAvailable = true,  CategoryId = 5 },
            new Product { Id = 12, Name = "Football",            Price = 24.99m,  Quantity = 12, IsAvailable = true,  CategoryId = 5 }
        );
    }
}
