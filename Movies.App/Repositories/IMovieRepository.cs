using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Movies.App.Models;

namespace Movies.App.Repositories;

public interface IMovieRepository
{
    Task<bool> CreateAsync(Movie movie);
    Task<Movie?> GetByIdAsync(Guid id);
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<bool> UpdateAsync(Movie movie);
    Task<bool> DeleteByIdAsync(Guid id);
}