namespace EFCore.Models.S4;

public class CustomerAccount
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;
    public Account Account { get; set; } = null!;

    public OwnershipType OwnershipType { get; set; }
    public DateOnly OwnershipStartDate { get; set; }
    public AccountStatus AccountStatus { get; set; }
}
