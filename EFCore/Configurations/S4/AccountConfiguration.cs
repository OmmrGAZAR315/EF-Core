using EFCore.Models.S4;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S4;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts", t =>
            t.HasCheckConstraint("CK_Accounts_CurrentBalance", "[CurrentBalance] >= 0"));

        builder.HasKey(a => a.AccountNumber);
        builder.Property(a => a.AccountNumber)
            .HasMaxLength(20);

        builder.Property(a => a.CurrentBalance)
            .HasPrecision(18, 2);

        builder.Property(a => a.AccountType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(a => a.Branch)
            .WithMany(b => b.Accounts)
            .HasForeignKey(a => a.BranchCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Account { AccountNumber = "2001-CUR", AccountType = AccountType.Current,  CurrentBalance = 15400m,  OpeningDate = new DateOnly(2020, 2, 10), BranchCode = "CAI-01" },
            new Account { AccountNumber = "3000-BUS", AccountType = AccountType.Business, CurrentBalance = 120000m, OpeningDate = new DateOnly(2019, 7, 1),  BranchCode = "ALX-01" },
            new Account { AccountNumber = "4001-SAV", AccountType = AccountType.Savings,  CurrentBalance = 52000m,  OpeningDate = new DateOnly(2021, 11, 5), BranchCode = "GIZ-01" }
        );
    }
}
