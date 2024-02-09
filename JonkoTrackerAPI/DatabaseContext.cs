using JonkoTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JonkoTrackerAPI;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Jonko> Jonkos { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // User friends
        builder.Entity<User>()
            .HasMany(u => u.Friends)
            .WithMany(u => u.FriendsOf)
            .UsingEntity(join => join.ToTable("Friends"));

        // User sessions
        builder.Entity<User>()
            .HasMany(u => u.Sessions)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);
    
        // Session jonkos
        builder.Entity<Session>()
            .HasMany(s => s.Jonkos)
            .WithOne(j => j.Session)
            .HasForeignKey(j => j.SessionId);

        // Jonko ingredients
        builder.Entity<Jonko>()
            .HasMany(j => j.Ingredients)
            .WithOne(i => i.Jonko)
            .HasForeignKey(i => i.JonkoId);
    }
}