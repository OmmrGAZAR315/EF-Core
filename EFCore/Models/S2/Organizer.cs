using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Models.S2;

[Table("EventOrganizers")]
public class Organizer
{
    [Key] public int Id { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; } = null!;
    [MaxLength(150)] public string? CompanyName { get; set; } = null;
    public bool IsVerified { get; set; }

    [InverseProperty(nameof(OrganizerProfile.Organizer))]
    public OrganizerProfile? Profile { get; set; } = null!;
    
    [InverseProperty(nameof(Event.Organizer))]
    public ICollection<Event> Events { get; set; } = new HashSet<Event>();
}