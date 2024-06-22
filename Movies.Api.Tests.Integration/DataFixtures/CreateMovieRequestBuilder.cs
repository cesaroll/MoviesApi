using Bogus;
using Movies.Contracts.Requests;

namespace Movies.Api.Tests.Integration.DataFixtures;

public class CreateMovieRequestBuilder
{
    private string? _title;
    private int _yeaOfRelease;
    private List<string> _genres = new();
    
    public static CreateMovieRequestBuilder Build()
    {
        return new CreateMovieRequestBuilder();
    }
    
    public static CreateMovieRequest BuildOne()
    {
        var faker = new Faker();
        return new CreateMovieRequest
        {
            Title = faker.Random.Words(3),
            YearOfRelease = faker.Random.Int(1995, 2015),
            Genres = new List<string> {faker.Lorem.Word(), faker.Lorem.Word()}
        };
    }
    
    public CreateMovieRequestBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }
    
    public CreateMovieRequestBuilder WithYearOfRelease(int yearOfRelease)
    {
        _yeaOfRelease = yearOfRelease;
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
            Title = _title,
            YearOfRelease = _yeaOfRelease,
            Genres = _genres
        };
    }
}