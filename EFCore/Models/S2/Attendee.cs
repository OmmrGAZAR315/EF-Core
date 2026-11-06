using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Models.S2;

public class Attendee
{
    [Key] public int Id { get; set; }
    [Required, MaxLength(150)] public string FullName { get; set; } = null!;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = null!;

    [Required] public Address Address { get; set; } = null!;

    [InverseProperty(nameof(Badge.Attendee))]
    public Badge? Badge { get; set; }

    [NotMapped]
    public string FullAddress => $"{Address.Street}, {Address.City}, {Address.Country}, {Address.PostalCode}";
}