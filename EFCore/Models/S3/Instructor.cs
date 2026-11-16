namespace EFCore.Models.S3;

public class Instructor
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Bio { get; set; }
    public DateTime JoinedAt { get; set; }

    public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}
