using Movies.App.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mappers;

public static class MovieMappers
{
    public static Movie ToMovie(this CreateMovieRequest request) => new Movie()
    {
        Id = Guid.NewGuid(),
        Title = request.Title,
        YearOfRelease = request.YearOfRelease,
        Genres = request.Genres.ToList()
    };
    
    public static MovieResponse ToMovieResponse(this Movie movie) => new MovieResponse()
    {
        Id = movie.Id,
        Title = movie.Title,
        YearOfRelease = movie.YearOfRelease,
        Genres = movie.Genres
    };
}