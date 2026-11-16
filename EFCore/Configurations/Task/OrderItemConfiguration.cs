using EFCore.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.Task;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
          .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(oi => oi.OrderId);
        
        builder.HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
          .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(oi => oi.ProductId);
        
        builder.Property(oi => oi.UnitPrice).HasPrecision(10, 2);
        builder.Property(oi => oi.Discount).HasPrecision(5, 2); // percentage, e.g. 10 = 10%
        
        builder.HasKey(oi => new { oi.OrderId, oi.ProductId });
        
        builder.HasData(
            // ORD-1001 (John)
            new OrderItem { OrderId = 1, ProductId = 1,  Quantity = 1, UnitPrice = 999.99m, Discount = 0 },
            new OrderItem { OrderId = 1, ProductId = 4,  Quantity = 2, UnitPrice = 19.99m,  Discount = 5 },
            // ORD-1002 (Jane)
            new OrderItem { OrderId = 2, ProductId = 2,  Quantity = 1, UnitPrice = 499.99m, Discount = 10 },
            new OrderItem { OrderId = 2, ProductId = 7,  Quantity = 3, UnitPrice = 14.99m,  Discount = 0 },
            // ORD-1003 (John)
            new OrderItem { OrderId = 3, ProductId = 5,  Quantity = 2, UnitPrice = 39.99m,  Discount = 0 },
            new OrderItem { OrderId = 3, ProductId = 6,  Quantity = 1, UnitPrice = 129.99m, Discount = 15 },
            // ORD-1004 (Ahmed)
            new OrderItem { OrderId = 4, ProductId = 9,  Quantity = 1, UnitPrice = 89.99m,  Discount = 0 },
            new OrderItem { OrderId = 4, ProductId = 8,  Quantity = 2, UnitPrice = 24.99m,  Discount = 0 },
            new OrderItem { OrderId = 4, ProductId = 7,  Quantity = 1, UnitPrice = 14.99m,  Discount = 0 },
            // ORD-1005 (Sara)
            new OrderItem { OrderId = 5, ProductId = 11, Quantity = 2, UnitPrice = 29.99m,  Discount = 5 },
            new OrderItem { OrderId = 5, ProductId = 12, Quantity = 1, UnitPrice = 24.99m,  Discount = 0 },
            // ORD-1006 (Jane)
            new OrderItem { OrderId = 6, ProductId = 3,  Quantity = 1, UnitPrice = 79.99m,  Discount = 0 },
            // ORD-1007 (Ahmed)
            new OrderItem { OrderId = 7, ProductId = 1,  Quantity = 1, UnitPrice = 999.99m, Discount = 5 },
            new OrderItem { OrderId = 7, ProductId = 2,  Quantity = 2, UnitPrice = 499.99m, Discount = 0 },
            // ORD-1008 (John)
            new OrderItem { OrderId = 8, ProductId = 4,  Quantity = 3, UnitPrice = 19.99m,  Discount = 0 },
            new OrderItem { OrderId = 8, ProductId = 10, Quantity = 1, UnitPrice = 59.99m,  Discount = 20 }
        );
    }
}
