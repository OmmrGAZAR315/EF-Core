using System.ComponentModel.DataAnnotations;

namespace EFCore.Models.S1;

public class Author
{
    [Key] public int Id { get; set; }
    [Required, StringLength(100)] public string FirstName { get; set; }
    [Required, StringLength(100)] public string LastName { get; set; }
    [EmailAddress, StringLength(100)] public string Email { get; set; }
    [StringLength(100)] public string Bio { get; set; }
    [Required] public DateTime DOB { get; set; }
}
