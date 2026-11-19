using EFCore.Models.S4;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S4;

public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
{
    public void Configure(EntityTypeBuilder<CustomerAccount> builder)
    {
        builder.ToTable("CustomerAccounts");

        builder.HasKey(ca => new { ca.CustomerId, ca.AccountNumber });

        builder.Property(ca => ca.OwnershipType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(ca => ca.AccountStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(ca => ca.Customer)
            .WithMany(c => c.CustomerAccounts)
            .HasForeignKey(ca => ca.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ca => ca.Account)
            .WithMany(a => a.CustomerAccounts)
            .HasForeignKey(ca => ca.AccountNumber)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new CustomerAccount { CustomerId = 1, AccountNumber = "2001-CUR", OwnershipType = OwnershipType.Primary,  OwnershipStartDate = new DateOnly(2020, 2, 10), AccountStatus = AccountStatus.Active },
            new CustomerAccount { CustomerId = 2, AccountNumber = "3000-BUS", OwnershipType = OwnershipType.Primary,  OwnershipStartDate = new DateOnly(2019, 7, 1),  AccountStatus = AccountStatus.Active },
            new CustomerAccount { CustomerId = 3, AccountNumber = "4001-SAV", OwnershipType = OwnershipType.Primary,  OwnershipStartDate = new DateOnly(2021, 11, 5), AccountStatus = AccountStatus.Active },
            new CustomerAccount { CustomerId = 1, AccountNumber = "4001-SAV", OwnershipType = OwnershipType.CoHolder, OwnershipStartDate = new DateOnly(2022, 1, 15), AccountStatus = AccountStatus.Active }
        );
    }
}
