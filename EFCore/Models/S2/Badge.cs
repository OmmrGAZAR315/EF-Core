using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Models.S2;

public enum BadgeTier
{
    Standard,
    VIP
}

public class Badge
{
    public int Id { get; set; }
    public int Number { get; set; }
    public DateTime IssuedOn { get; set; }
    public BadgeTier BadgeTier { get; set; }
    
    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = null!;
}