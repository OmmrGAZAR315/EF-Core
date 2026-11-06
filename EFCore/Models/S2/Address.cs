using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Models.S2;

[Owned]
public class Address
{
    [Required, MaxLength(150)] public string Street { get; set; } = null!;
    [Required, MaxLength(100)] public string City { get; set; } = null!;
    [Required, MaxLength(100)] public string Country { get; set; } = null!;

    [Required, MaxLength(20), Column(name: "Address_ZipCode")]
    public string PostalCode { get; set; } = null!;
}