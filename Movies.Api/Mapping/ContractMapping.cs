using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity.Data;
using Movies.App.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping;

public static partial class ContractMapping
{
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        var id = Guid.NewGuid(); 
        
        return new Movie()
        {
            Id = id,
            Title = request.Title,
            Slug = GenerateSlug(request.Title, request.YearOfRelease),
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.Select(x => new Genre() { MovieId = id, Name = x }).ToList()
        };
    }
    
    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id) => new Movie()
    {
        Id = id,
        Title = request.Title,
        Slug = GenerateSlug(request.Title, request.YearOfRelease),
        YearOfRelease = request.YearOfRelease,
        Genres = request.Genres.Select(x => new Genre() { MovieId = id, Name = x }).ToList()
    };
    
    public static MovieResponse MapToMovieResponse(this Movie movie) => new MovieResponse()
    {
        Id = movie.Id,
        Title = movie.Title,
        Slug = movie.Slug,
        YearOfRelease = movie.YearOfRelease,
        Genres = movie.Genres.Select(x => x.Name)
    };

    public static MoviesResponse MapToMoviesResponse(this IEnumerable<Movie> movies) =>
        new()
        {
            Items = movies.Select(MapToMovieResponse)
        };
    
    
    public static string GenerateSlug(string title, int yearOfRelease)
    {
        var sluggedTitle = SlugRegex().Replace(title, string.Empty)
            .ToLower().Replace(" ", "-");

        return $"{sluggedTitle}-{yearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}