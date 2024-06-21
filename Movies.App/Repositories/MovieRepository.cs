using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies.App.Models;

namespace Movies.App.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();
    
    public Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var movie = _movies.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(movie);
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        var movie = _movies.FirstOrDefault(x => x.Slug == slug);
        return Task.FromResult(movie);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        var index = _movies.FindIndex(x => x.Id == movie.Id);
        if (index == -1)
        {
            return Task.FromResult(false);
        }

        _movies[index] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removedCount = _movies.RemoveAll(x => x.Id == id);
        return Task.FromResult(removedCount > 0);
    }
}