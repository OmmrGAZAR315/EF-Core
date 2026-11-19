using EFCore.Models.S4;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S4;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(b => b.Code);
        builder.Property(b => b.Code)
            .HasMaxLength(10);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(b => b.Manager)
            .WithOne(m => m.Branch)
            .HasForeignKey<Branch>(b => b.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(b => b.ManagerId)
            .IsUnique();

        builder.HasData(
            new Branch { Code = "CAI-01", Name = "Cairo Main Branch",  Address = "15 Tahrir Square, Cairo",     PhoneNumber = "0223456789", ManagerId = 1 },
            new Branch { Code = "ALX-01", Name = "Alexandria Branch",  Address = "22 Corniche Rd, Alexandria",  PhoneNumber = "034567890",  ManagerId = 2 },
            new Branch { Code = "GIZ-01", Name = "Giza Branch",        Address = "8 Pyramids St, Giza",         PhoneNumber = "0237654321", ManagerId = 3 }
        );
    }
}
