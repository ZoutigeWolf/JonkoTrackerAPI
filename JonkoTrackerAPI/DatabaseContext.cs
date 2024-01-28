using JonkoTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JonkoTrackerAPI;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasMany(u => u.Friends)
            .WithMany(u => u.FriendsOf)
            .UsingEntity(join => join.ToTable("Friends"));
    }
}