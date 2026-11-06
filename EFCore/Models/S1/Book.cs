using System.ComponentModel.DataAnnotations;

namespace EFCore.Models.S1;

public class Book
{
    [Key] public int Id { get; set; }
    [Required, StringLength(13)] public string ISBN { get; set; }
    [Required, StringLength(100)] public string Title { get; set; }
    [Range(0, double.MaxValue)] public decimal Price { get; set; }
    [Range(1, int.MaxValue)] public int PageCount { get; set; }
    [DataType(DataType.Date)] public DateTime PublishYear { get; set; }
    [Required] public bool IsInStock { get; set; }
}
