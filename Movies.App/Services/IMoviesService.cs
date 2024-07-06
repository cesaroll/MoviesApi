using Movies.App.Models;

namespace Movies.App.Services;

public interface IMoviesService
{
    Task<bool> CreateAsync(MovieContext context);
    Task<Movie?> GetByIdAsync(IdContext context);
    Task<Movie?> GetBySlugAsync(SlugContext constext);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct);
    Task<Movie?> UpdateAsync(MovieContext context);
    Task<bool> DeleteByIdAsync(IdContext context);
}

public record MovieContext(Movie Movie, CancellationToken Ct);
public record IdContext(Guid Id, CancellationToken Ct);
public record SlugContext(string Slug, CancellationToken Ct);