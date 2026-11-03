using EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
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