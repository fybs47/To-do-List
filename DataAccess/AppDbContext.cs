using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        modelBuilder
            .Entity<TaskItem>()
            .Property(e => e.Status)
            .HasConversion<string>();
        
        modelBuilder
            .Entity<TaskItem>()
            .Property(e => e.Priority)
            .HasConversion<string>();
        
        modelBuilder
            .Entity<User>()
            .Property(e => e.Role)
            .HasConversion<string>();
    }
}