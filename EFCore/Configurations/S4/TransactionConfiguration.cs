using EFCore.Models.S4;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Configurations.S4;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions", t =>
            t.HasCheckConstraint("CK_Transactions_Amount", "[Amount] > 0"));

        builder.HasKey(t => t.TransactionNumber);

        builder.Property(t => t.TransactionDate)
            .HasColumnType("datetime2");

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2);

        builder.Property(t => t.TransactionType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.Note)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountNumber)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new Transaction { TransactionNumber = 1, AccountNumber = "2001-CUR", TransactionDate = new DateTime(2025, 1, 5),  Amount = 20000m,  TransactionType = TransactionType.Deposit,    Note = "Initial deposit" },
            new Transaction { TransactionNumber = 2, AccountNumber = "2001-CUR", TransactionDate = new DateTime(2025, 2, 10), Amount = 4600m,   TransactionType = TransactionType.Withdrawal, Note = "Rent payment" },
            new Transaction { TransactionNumber = 3, AccountNumber = "3000-BUS", TransactionDate = new DateTime(2025, 1, 15), Amount = 150000m, TransactionType = TransactionType.Deposit,    Note = "Capital injection" },
            new Transaction { TransactionNumber = 4, AccountNumber = "3000-BUS", TransactionDate = new DateTime(2025, 3, 1),  Amount = 30000m,  TransactionType = TransactionType.Transfer,   Note = "Supplier payment" },
            new Transaction { TransactionNumber = 5, AccountNumber = "4001-SAV", TransactionDate = new DateTime(2025, 1, 20), Amount = 50000m,  TransactionType = TransactionType.Deposit,    Note = "Opening deposit" },
            new Transaction { TransactionNumber = 6, AccountNumber = "4001-SAV", TransactionDate = new DateTime(2025, 4, 1),  Amount = 2000m,   TransactionType = TransactionType.Deposit,    Note = "Monthly savings" },
            new Transaction { TransactionNumber = 7, AccountNumber = "4001-SAV", TransactionDate = new DateTime(2025, 5, 1),  Amount = 2000m,   TransactionType = TransactionType.Deposit,    Note = null },
            new Transaction { TransactionNumber = 8, AccountNumber = "4001-SAV", TransactionDate = new DateTime(2025, 6, 12), Amount = 2000m,   TransactionType = TransactionType.Withdrawal, Note = "ATM withdrawal" }
        );
    }
}
