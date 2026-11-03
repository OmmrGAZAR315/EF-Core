using System.ComponentModel.DataAnnotations;

namespace EFCore.Models;

public class Book
{
    [Key] public int ISBN { get; set; }
    [Required, StringLength(100)] public string Title { get; set; }
    [Range(0, double.MaxValue)] public decimal Price { get; set; }
    [Range(1, int.MaxValue)] public int PageCount { get; set; }
    [DataType(DataType.Date)] public DateTime PublishYear { get; set; }
    [Required] public bool IsInStock { get; set; }
}
