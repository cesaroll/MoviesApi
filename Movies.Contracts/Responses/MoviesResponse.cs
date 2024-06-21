namespace Movies.Contracts.Responses;

public class MoviesResponse
{
    public required IEnumerable<MovieResponse> Type { get; init; } = [];
}