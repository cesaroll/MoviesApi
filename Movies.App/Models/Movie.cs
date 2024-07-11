namespace Movies.App.Models;

public partial class Movie
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string Slug { get; set; }
    public required int YearOfRelease { get; set; }
    public required List<Genre> Genres { get; init; } = new();
    public required List<Rating> Ratings { get; init; } = new();
}