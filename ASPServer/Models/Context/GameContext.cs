using Microsoft.EntityFrameworkCore;
using Models.Model;

namespace Models.Contexts;

public sealed class GameContext : DbContext
{
    public GameContext(DbContextOptions<GameContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserScore>();
    }
}