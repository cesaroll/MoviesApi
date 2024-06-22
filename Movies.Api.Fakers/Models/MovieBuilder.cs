using Bogus;
using Movies.App.Models;

namespace Movies.Api.Fakers.Models;

public class MovieBuilder
{
    private Guid? _id;
    private string? _title;
    private int _yearOfRelease = 0;
    private List<string> _genres = new();
    
    private static readonly Faker _faker = new Faker();

    public static MovieBuilder Build() => new MovieBuilder();
    
    public MovieBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }
    
    public MovieBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }
    
    public MovieBuilder WithYearOfRelease(int yearOfRelease)
    {
        _yearOfRelease = yearOfRelease;
        return this;
    }
    
    public MovieBuilder WithGenres(List<string> genres)
    {
        _genres = genres;
        return this;
    }
    
    public MovieBuilder WithGenre(string genre)
    {
        _genres.Add(genre);
        return this;
    }
    
    public Movie Create()
    {
        var genres = _genres.Select(g => new Genre { Name = g }).ToList();
        
        var movie = new Movie
        {
            Id = _id ?? _faker.Random.Guid(),
            Title = _title ?? _faker.Random.Words(3),
            YearOfRelease = _yearOfRelease > 0 ? _yearOfRelease : _faker.Random.Int(1995, 2015),
            Genres = genres
        };
        
        foreach (var genre in genres)
        {
            genre.Movie = movie;
            genre.MovieId = movie.Id;
        }

        return movie;
    }
    
    public static Movie CreateOne() => 
        MovieBuilder.Build()
            .WithGenre(_faker.Lorem.Word())
            .WithGenre(_faker.Lorem.Word())
            .Create();

    public static IList<Movie> CreateMany(int count = 3)
    {
        var movies = new List<Movie>();
        for (var i = 0; i < count; i++)
        {
            movies.Add(CreateOne());
        }
        return movies;
    }
    
}