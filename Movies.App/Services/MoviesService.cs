using Microsoft.EntityFrameworkCore;
using Movies.App.Database;
using Movies.App.Models;

namespace Movies.App.Services;

public class MoviesService : IMoviesService
{
    private readonly MoviesDbContext _context;

    public MoviesService(MoviesDbContext context)
    {
        _context = context;
    }


    public async Task<bool> CreateAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        return await _context.Movies
            .FindAsync(id);
    }

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        return await _context.Movies.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Slug == slug);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _context.Movies.AsNoTracking()
            .ToListAsync();
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
        var movieExists = await _context.Movies
            .AsNoTracking()
            .AnyAsync(x => x.Id == movie.Id);
        
        if (!movieExists)
            return null;
        
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
            return false;

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return true;
    }
}