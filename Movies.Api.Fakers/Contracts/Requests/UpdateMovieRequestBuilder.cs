using Bogus;
using Movies.Contracts.Requests;

namespace Movies.Api.Fakers.Contracts.Requests;

public class UpdateMovieRequestBuilder
{
    private static readonly Faker Faker = new Faker();
    
    private static readonly Faker<UpdateMovieRequest> Generator = new Faker<UpdateMovieRequest>()
        .RuleFor(x => x.Title, f => f.Random.Words(2))
        .RuleFor(x => x.YearOfRelease, f => f.Random.Int(1995, 2015))
        .RuleFor(x => x.Genres, f => f.Lorem.Words(2));

    public static UpdateMovieRequest CreateOne() => Generator.Generate();
}