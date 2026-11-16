using EFCore.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.Task;

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.HasKey(pt => new { pt.ProductId, pt.TagId });
        builder.HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTags)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(pt => pt.ProductId);
        builder.HasOne(pt => pt.Tag)
            .WithMany(t => t.ProductTags)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(pt => pt.TagId);

        builder.HasData(
            new ProductTag { ProductId = 1,  TagId = 1 }, // Laptop: New
            new ProductTag { ProductId = 1,  TagId = 3 }, // Laptop: Bestseller
            new ProductTag { ProductId = 2,  TagId = 1 }, // Smartphone: New
            new ProductTag { ProductId = 2,  TagId = 2 }, // Smartphone: Sale
            new ProductTag { ProductId = 3,  TagId = 2 }, // Headphones: Sale
            new ProductTag { ProductId = 4,  TagId = 2 }, // T-Shirt: Sale
            new ProductTag { ProductId = 4,  TagId = 4 }, // T-Shirt: Eco-Friendly
            new ProductTag { ProductId = 5,  TagId = 3 }, // Jeans: Bestseller
            new ProductTag { ProductId = 6,  TagId = 5 }, // Winter Jacket: Limited Edition
            new ProductTag { ProductId = 7,  TagId = 3 }, // Novel: Bestseller
            new ProductTag { ProductId = 7,  TagId = 6 }, // Novel: Gift Idea
            new ProductTag { ProductId = 8,  TagId = 6 }, // Cookbook: Gift Idea
            new ProductTag { ProductId = 9,  TagId = 1 }, // Coffee Maker: New
            new ProductTag { ProductId = 11, TagId = 4 }, // Yoga Mat: Eco-Friendly
            new ProductTag { ProductId = 12, TagId = 6 }  // Football: Gift Idea
            // Product 10 (Blender) has no tags on purpose.
        );
    }
}
