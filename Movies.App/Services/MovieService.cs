using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Movies.App.Database;
using Movies.App.Models;

namespace Movies.App.Services;

public class MovieService : IMovieService
{
    private readonly MoviesDbContext _dbContext;
    private readonly IValidator<Movie> _movieValidator;

    public MovieService(MoviesDbContext context, IValidator<Movie> movieValidator)
    {
        _dbContext = context;
        _movieValidator = movieValidator;
    }

    public async Task<bool> CreateAsync(MovieContext context)
    {
        await _movieValidator.ValidateAndThrowAsync(context.Movie, context.Ct);

        if (await MovieExists())
            return false;

        await _dbContext.Movies.AddAsync(context.Movie, context.Ct);
        await _dbContext.SaveChangesAsync(context.Ct);
        return true;

        async Task<bool> MovieExists()
        {
            var existingMovie = await _dbContext.Movies.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Slug == context.Movie.Slug, context.Ct);

            return existingMovie is not null;
        }
    }

    public async Task<Movie?> GetByIdAsync(MovieIdContext context)
    {
        return await _dbContext.Movies.AsNoTracking()
            .Include(m => m.Ratings)
            .FirstOrDefaultAsync(m => m.Id == context.Id, context.Ct);
    }

    public async Task<Movie?> GetBySlugAsync(SlugContext context)
    {
        return await _dbContext.Movies.AsNoTracking()
            .Include(m => m.Ratings)
            .FirstOrDefaultAsync(m => m.Slug == context.Slug, context.Ct);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct)
    {
        return await _dbContext.Movies.AsNoTracking()
            .Include(m => m.Ratings)
            .ToListAsync(ct);
    }

    public async Task<Movie?> UpdateAsync(MovieContext context)
    {
        await _movieValidator.ValidateAndThrowAsync(context.Movie, context.Ct);
        var movieExists = await _dbContext.Movies
            .AsNoTracking()
            .AnyAsync(x => x.Id == context.Movie.Id, context.Ct);

        if (!movieExists)
            return null;

        var movie = _dbContext.Movies.Update(context.Movie);
        await _dbContext.SaveChangesAsync(context.Ct);
        return movie.Entity;
    }

    public async Task<bool> DeleteByIdAsync(MovieIdContext context)
    {
        var movie = await _dbContext.Movies.FindAsync(context.Id, context.Ct);
        if (movie == null)
            return false;

        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync(context.Ct);

        return true;
    }
}
