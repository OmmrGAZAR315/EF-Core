using EFCore.Models.S4;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S4;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.NationalId)
            .IsRequired()
            .HasMaxLength(20);
        builder.HasIndex(c => c.NationalId)
            .IsUnique();

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.CustomerType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasData(
            new Customer { Id = 1, FullName = "Ahmed Ali",        NationalId = "29001011234567", DateOfBirth = new DateOnly(1990, 1, 1),   Email = "ahmed.ali@mail.com",     PhoneNumber = "01011111111", Address = "10 Nasr St, Cairo",          CustomerType = CustomerType.Individual },
            new Customer { Id = 2, FullName = "Nile Trading LLC", NationalId = "20100520001",    DateOfBirth = new DateOnly(2010, 5, 20),  Email = "info@niletrading.com",   PhoneNumber = "034440000",   Address = "5 Port St, Alexandria",      CustomerType = CustomerType.Business },
            new Customer { Id = 3, FullName = "Salma Youssef",    NationalId = "29505151234568", DateOfBirth = new DateOnly(1995, 5, 15),  Email = "salma.youssef@mail.com", PhoneNumber = "01022222222", Address = "3 Haram St, Giza",           CustomerType = CustomerType.Individual }
        );
    }
}
