namespace EFCore.Models.Task;

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
}