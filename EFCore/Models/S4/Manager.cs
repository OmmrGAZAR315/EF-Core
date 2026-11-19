namespace EFCore.Models.S4;

public class Manager
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateOnly HireDate { get; set; }

    public Branch? Branch { get; set; }
}
