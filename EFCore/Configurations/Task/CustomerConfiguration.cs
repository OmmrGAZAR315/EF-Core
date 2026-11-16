using EFCore.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.Task;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
      builder.HasMany(c => c.Orders)
          .WithOne(o => o.Customer)
          .OnDelete(DeleteBehavior.Restrict)
          .HasForeignKey(o => o.CustomerId);
      
      builder.Property(c => c.Name)
          .IsRequired()
          .HasMaxLength(100);
      
      builder.HasData(
          new Customer { Id = 1, Name = "John Doe",     Email = "john.doe@example.c`om",     PhoneNumber = "+20-100-000-0001", Address = "12 Tahrir St, Cairo" },
          new Customer { Id = 2, Name = "Jane Smith",   Email = "jane.smith@example.com",   PhoneNumber = "+20-100-000-0002", Address = "5 Corniche Rd, Alexandria" },
          new Customer { Id = 3, Name = "Ahmed Hassan", Email = "ahmed.hassan@example.com", PhoneNumber = "+20-100-000-0003", Address = "40 Nile St, Giza" },
          new Customer { Id = 4, Name = "Sara Ali",     Email = "sara.ali@example.com",     PhoneNumber = "+20-100-000-0004", Address = "8 Port Said St, Mansoura" },
          new Customer { Id = 5, Name = "Mike Brown",   Email = "mike.brown@example.com",   PhoneNumber = "+20-100-000-0005", Address = "77 Sea St, Hurghada" } // no orders on purpose
      );
    }
}
