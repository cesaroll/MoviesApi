using Bogus;
using Movies.Contracts.Requests;

namespace Movies.Api.Fakers.Contracts.Requests;

public class CreateMovieRequestBuilder
{
    private string? _title;
    private int _yearOfRelease = 0;
    private List<string> _genres = new();
    
    private static readonly Faker Faker = new Faker();
    
    private static readonly Faker<CreateMovieRequest> Generator = new Faker<CreateMovieRequest>()
        .RuleFor(x => x.Title, f => f.Random.Words(2))
        .RuleFor(x => x.YearOfRelease, f => f.Random.Int(1995, 2015))
        .RuleFor(x => x.Genres, f => f.Lorem.Words(2));

    public static CreateMovieRequest CreateOne() => Generator.Generate();
    
    public static CreateMovieRequestBuilder Build()
    {
        return new CreateMovieRequestBuilder();
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
        var movie = CreateOne();
        return new CreateMovieRequest
        {
            Title = _title ?? movie.Title,
            YearOfRelease = (_yearOfRelease > 0) ? _yearOfRelease : movie.YearOfRelease,
            Genres = _genres
        };
    }
}