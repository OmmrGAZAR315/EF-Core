// using EFCore.Configurations.S2;
// using EFCore.Configurations.Task;
// using EFCore.Models;
// using EFCore.Models.S1;
// using EFCore.Models.S2;
using EFCore.Configurations.S3;
using EFCore.Models.S3;
using Microsoft.EntityFrameworkCore;

namespace EFCore;

public class AppDbContext : DbContext
{
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    // public DbSet<Book> Books => Set<Book>();
    // public DbSet<Author> Authors => Set<Author>();
    // public DbSet<Category> Categories => Set<Category>();
    // public DbSet<Organizer> Organizers => Set<Organizer>();
    // public DbSet<Event> Events => Set<Event>();
    // public DbSet<OrganizerProfile> OrganizerProfiles => Set<OrganizerProfile>();
    // public DbSet<Attendee> Attendees => Set<Attendee>();
    // public DbSet<Badge> Badges => Set<Badge>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=EFCore;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InstructorConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());

        // modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        // modelBuilder.ApplyConfiguration(new OrderConfiguration());
        // modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        // modelBuilder.ApplyConfiguration(new ProductConfiguration());
        // modelBuilder.ApplyConfiguration(new TagConfiguration());
        // modelBuilder.ApplyConfiguration(new ProductTagConfiguration());
        // modelBuilder.ApplyConfiguration(new CategoryConfiguration());

        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        // modelBuilder.ApplyConfiguration(new OrganizerProfileConfiguration());
        // modelBuilder.ApplyConfiguration(new BadgeConfiguration());

        // modelBuilder.Entity<Event>().HasOne(e => e.Organizer)
            // .WithMany(o => o.Events)
            // .HasForeignKey(e => e.OrganizerId);

        // modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        // modelBuilder.ApplyConfiguration(new BookConfiguration());
        // modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}
