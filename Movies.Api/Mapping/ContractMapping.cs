using System.Text.RegularExpressions;
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
            Genres = request.Genres.Select(x => new Genre() { MovieId = id, Name = x }).ToList(),
            Ratings = []
        };
    }
    
    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id) => new Movie()
    {
        Id = id,
        Title = request.Title,
        Slug = GenerateSlug(request.Title, request.YearOfRelease),
        YearOfRelease = request.YearOfRelease,
        Genres = request.Genres.Select(x => new Genre() { MovieId = id, Name = x }).ToList(),
        Ratings = []
    };
    
    public static MovieResponse MapToMovieResponse(this Movie movie, Guid? userId) => 
        new()
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            Rating = movie.Ratings.GetAverageRating(),
            UserRating = movie.Ratings.GetUserRating(userId),
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres.Select(x => x.Name)
        };

    private static float? GetAverageRating(this List<Rating>? ratings) =>
        ratings?.Count == 0
            ? null
            : (float?)ratings.Select(r => r.UserRating).Average();

    private static int? GetUserRating(this List<Rating>? ratings, Guid? userId) =>
        userId is null || ratings?.Count == 0
            ? null
            : ratings?.FirstOrDefault(r => r.UserId == userId)?.UserRating;
    
    public static MoviesResponse MapToMoviesResponse(this IEnumerable<Movie> movies, Guid? userId) =>
        new()
        {
            Items = movies.Select(m => m.MapToMovieResponse(userId))
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