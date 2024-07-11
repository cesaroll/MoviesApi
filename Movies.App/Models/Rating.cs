namespace Movies.App.Models;

public class Rating
{
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; }
    public int UserRating { get; set; }
    
    public Movie Movie { get; set; }
}