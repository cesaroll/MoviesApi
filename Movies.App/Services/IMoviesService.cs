using Movies.App.Models;

namespace Movies.App.Services;

public interface IMoviesService
{
    Task<bool> CreateAsync(Movie movie);
    Task<Movie?> GetByIdAsync(Guid id);
    Task<Movie?> GetBySlugAsync(string slug);
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> UpdateAsync(Movie movie);
    Task<bool> DeleteByIdAsync(Guid id);
}