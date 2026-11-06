using System.ComponentModel.DataAnnotations;

namespace EFCore.Models.S2;

public class OrganizerProfile
{
    public int Id { get; set; }
    public string Bio { get; set; } = null!;
    public string? Link { get; set; }
    public string Logo { get; set; } = null!;
    public int OrganizerId { get; set; }
    public Organizer Organizer { get; set; } = null!;
}