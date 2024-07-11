namespace Movies.App.Services;

public interface IRatingService
{
    Task<bool> RateMovieAsync(RatingIdContext context);
}