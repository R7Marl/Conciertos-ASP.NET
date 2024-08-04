using Microsoft.EntityFrameworkCore;
using WebApplication2.Entity;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Users> Users { get; set; }
    public DbSet<Conciertos> Conciertos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>().ToTable("Users");
        modelBuilder.Entity<Conciertos>().ToTable("Conciertos");
    }

}
