using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Movies.App.Database;
using Movies.App.Models;

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

    public async Task<bool> CreateAsync(MovieContext context)
    {
        await _movieValidator.ValidateAndThrowAsync(context.Movie, context.Ct);
        
        if (await MovieExists())
            return false;
        
        await _context.Movies.AddAsync(context.Movie, context.Ct);
        await _context.SaveChangesAsync(context.Ct);
        return true;
        
        async Task<bool> MovieExists()
        {
            var existingMovie = await _context.Movies.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Slug == context.Movie.Slug, context.Ct);

            if (existingMovie is null)
                return false;
            
            return true;
        }
    }

    public async Task<Movie?> GetByIdAsync(IdContext context)
    {
        return await _context.Movies
            .FindAsync(context.Id, context.Ct);
    }

    public async Task<Movie?> GetBySlugAsync(SlugContext context)
    {
        return await _context.Movies.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Slug == context.Slug, context.Ct);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Movies.AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Movie?> UpdateAsync(MovieContext context)
    {
        await _movieValidator.ValidateAndThrowAsync(context.Movie, context.Ct);
        var movieExists = await _context.Movies
            .AsNoTracking()
            .AnyAsync(x => x.Id == context.Movie.Id, context.Ct);
        
        if (!movieExists)
            return null;
        
        var movie = _context.Movies.Update(context.Movie);
        await _context.SaveChangesAsync(context.Ct);
        return movie.Entity;
    }

    public async Task<bool> DeleteByIdAsync(IdContext context)
    {
        var movie = await _context.Movies.FindAsync(context.Id, context.Ct);
        if (movie == null)
            return false;

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync(context.Ct);

        return true;
    }
}