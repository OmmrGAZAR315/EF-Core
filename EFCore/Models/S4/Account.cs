namespace EFCore.Models.S4;

public class Account
{
    public string AccountNumber { get; set; } = null!;
    public decimal CurrentBalance { get; set; }
    public AccountType AccountType { get; set; }
    public DateOnly OpeningDate { get; set; }

    public string BranchCode { get; set; } = null!;
    public Branch Branch { get; set; } = null!;

    public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new HashSet<CustomerAccount>();
    public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
}
