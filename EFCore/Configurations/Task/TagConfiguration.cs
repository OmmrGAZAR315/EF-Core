using EFCore.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.Task;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
      builder.HasMany(t => t.ProductTags)
          .WithOne(pt => pt.Tag)
          .OnDelete(DeleteBehavior.Restrict)
          .HasForeignKey(pt => pt.TagId);
      
      builder.Property(t => t.Name)
          .IsRequired()
          .HasMaxLength(100);
      
      builder.HasIndex(t => t.Name)
          .IsUnique();

      builder.HasData(
          new Tag { Id = 1, Name = "New" },
          new Tag { Id = 2, Name = "Sale" },
          new Tag { Id = 3, Name = "Bestseller" },
          new Tag { Id = 4, Name = "Eco-Friendly" },
          new Tag { Id = 5, Name = "Limited Edition" },
          new Tag { Id = 6, Name = "Gift Idea" }
      );
    }
}
