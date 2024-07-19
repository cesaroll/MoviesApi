using Movies.App.Models;

namespace Movies.App.Services;

public interface IRatingService
{
    Task<bool> RateMovieAsync(RatingIdContext context);
    Task<bool> DeleteRatingAsync(RatingIdContext context);
    Task<List<Rating>> GetUserRatings(Guid userId, CancellationToken ct);
}
