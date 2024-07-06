using Movies.App.Models;

namespace Movies.App.Services;

public interface IMoviesService
{
    Task<bool> CreateAsync(Movie movie, CancellationToken ct);
    Task<Movie?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Movie?> GetBySlugAsync(string slug, CancellationToken ct);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken ct);
    Task<Movie?> UpdateAsync(Movie movie, CancellationToken ct);
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken ct);
}