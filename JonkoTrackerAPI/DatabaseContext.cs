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
        
    }
}