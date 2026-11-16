using EFCore.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.Task;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(o => o.CustomerId);

        builder.Property(o => o.OrderDate).HasColumnType("datetime2");

        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(oi => oi.OrderId);

        builder.HasData(
            new Order { Id = 1, Name = "ORD-1001", CustomerId = 1, OrderDate = new DateTime(2025, 1, 5) },
            new Order { Id = 2, Name = "ORD-1002", CustomerId = 2, OrderDate = new DateTime(2025, 1, 20) },
            new Order { Id = 3, Name = "ORD-1003", CustomerId = 1, OrderDate = new DateTime(2025, 2, 11) },
            new Order { Id = 4, Name = "ORD-1004", CustomerId = 3, OrderDate = new DateTime(2025, 3, 2) },
            new Order { Id = 5, Name = "ORD-1005", CustomerId = 4, OrderDate = new DateTime(2025, 3, 15) },
            new Order { Id = 6, Name = "ORD-1006", CustomerId = 2, OrderDate = new DateTime(2025, 4, 1) },
            new Order { Id = 7, Name = "ORD-1007", CustomerId = 3, OrderDate = new DateTime(2025, 4, 22) },
            new Order { Id = 8, Name = "ORD-1008", CustomerId = 1, OrderDate = new DateTime(2025, 5, 10) }
        );
    }
}
