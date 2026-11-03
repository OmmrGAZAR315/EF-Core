using System.ComponentModel.DataAnnotations;

namespace EFCore.Models;

public class Category
{
    [Key] public int Id { get; set; }
    [Required, StringLength(100)] public string Name { get; set; }
    [StringLength(200)] public string Description { get; set; }
    [Required] public bool IsActivate { get; set; }
}
