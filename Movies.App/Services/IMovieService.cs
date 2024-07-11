using Movies.App.Models;

namespace Movies.App.Services;

public interface IMovieService
{
    Task<bool> CreateAsync(MovieContext context);
    Task<Movie?> GetByIdAsync(MovieIdContext context);
    Task<Movie?> GetBySlugAsync(SlugContext constext);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct);
    Task<Movie?> UpdateAsync(MovieContext context);
    Task<bool> DeleteByIdAsync(MovieIdContext context);
}