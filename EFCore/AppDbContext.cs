using Microsoft.EntityFrameworkCore;

namespace EFCore;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=EFCore;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        // modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        // modelBuilder.ApplyConfiguration(new BookConfiguration());
        // modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}