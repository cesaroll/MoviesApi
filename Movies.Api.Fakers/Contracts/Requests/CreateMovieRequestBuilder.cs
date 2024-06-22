using Bogus;
using Movies.Contracts.Requests;

namespace Movies.Api.Fakers.Contracts.Requests;

public class CreateMovieRequestBuilder
{
    private string? _title;
    private int _yearOfRelease = 0;
    private List<string> _genres = new();
    
    private static Faker _faker = new Faker();
    
    public static CreateMovieRequestBuilder Build()
    {
        return new CreateMovieRequestBuilder();
    }
    
    public static CreateMovieRequest CreateOne()
    {
        return new CreateMovieRequest
        {
            Title = _faker.Random.Words(3),
            YearOfRelease = _faker.Random.Int(1995, 2015),
            Genres = new List<string> {_faker.Lorem.Word(), _faker.Lorem.Word()}
        };
    }
    
    public CreateMovieRequestBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }
    
    public CreateMovieRequestBuilder WithYearOfRelease(int yearOfRelease)
    {
        _yearOfRelease = yearOfRelease;
        return this;
    }
    
    public CreateMovieRequestBuilder WithGenres(List<string> genres)
    {
        _genres = genres;
        return this;
    }
    
    public CreateMovieRequestBuilder WithGenre(string genre)
    {
        _genres.Add(genre);
        return this;
    }
    public CreateMovieRequest Create()
    {
        return new CreateMovieRequest
        {
            Title = _title ?? _faker.Random.Words(3),
            YearOfRelease = (_yearOfRelease > 0) ? _yearOfRelease : _faker.Random.Int(1995, 2015),
            Genres = _genres
        };
    }
}