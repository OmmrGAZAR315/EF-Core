using EFCore.Models.S4;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S4;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("Managers");

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(150);
        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.Property(m => m.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasData(
            new Manager { Id = 1, FullName = "Hossam Fathy",  Email = "hossam.fathy@nationalbank.com",  PhoneNumber = "01000000001", HireDate = new DateOnly(2015, 3, 1) },
            new Manager { Id = 2, FullName = "Dina Mahmoud",  Email = "dina.mahmoud@nationalbank.com",  PhoneNumber = "01000000002", HireDate = new DateOnly(2017, 6, 15) },
            new Manager { Id = 3, FullName = "Karim Saad",    Email = "karim.saad@nationalbank.com",    PhoneNumber = "01000000003", HireDate = new DateOnly(2019, 9, 10) }
        );
    }
}
