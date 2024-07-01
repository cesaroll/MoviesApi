namespace Movies.App.Models;

public class Genre
{
    public Guid MovieId { get; set; }
    public required string Name { get; set; } = string.Empty;
    public Movie Movie { get; set; }
}