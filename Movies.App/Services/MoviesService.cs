using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Movies.App.Database;
using Movies.App.Models;
using Movies.App.Validators;

namespace Movies.App.Services;

public class MoviesService : IMoviesService
{
    private readonly MoviesDbContext _context;
    private readonly IValidator<Movie> _movieValidator;

    public MoviesService(MoviesDbContext context, IValidator<Movie> movieValidator)
    {
        _context = context;
        _movieValidator = movieValidator;
    }


    public async Task<bool> CreateAsync(Movie movie, CancellationToken ct)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, ct);
        
        if (await MovieExists())
            return false;
        
        await _context.Movies.AddAsync(movie, ct);
        await _context.SaveChangesAsync(ct);
        return true;
        
        async Task<bool> MovieExists()
        {
            var existingMovie = await _context.Movies.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Slug == movie.Slug, ct);

            if (existingMovie is null)
                return false;
            
            return true;
        }
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Movies
            .FindAsync(id, ct);
    }

    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken ct)
    {
        return await _context.Movies.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Slug == slug, ct);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Movies.AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken ct)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, ct);
        var movieExists = await _context.Movies
            .AsNoTracking()
            .AnyAsync(x => x.Id == movie.Id, ct);
        
        if (!movieExists)
            return null;
        
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync(ct);
        return movie;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken ct)
    {
        var movie = await _context.Movies.FindAsync(id, ct);
        if (movie == null)
            return false;

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync(ct);

        return true;
    }
}