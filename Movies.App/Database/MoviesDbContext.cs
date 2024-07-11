using Microsoft.EntityFrameworkCore;
using Movies.App.Models;

namespace Movies.App.Database;

public sealed class MoviesDbContext : DbContext
{
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
    {
    }
    
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>()
            .HasKey(g => new { g.MovieId, g.Name });
        
        modelBuilder.Entity<Rating>()
            .HasKey(r => new { r.MovieId, r.UserId });
    }
}