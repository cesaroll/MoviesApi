using Microsoft.EntityFrameworkCore;
using Movies.App.Database;
using Movies.App.Models;

namespace Movies.App.Services;

public class RatingService : IRatingService
{
    private readonly MoviesDbContext _dbContext;

    public RatingService(MoviesDbContext context)
    {
        _dbContext = context;
    }

    public async Task<bool> RateMovieAsync(RatingIdContext context)
    {
        var movie = await _dbContext.Movies
            .Include(m => m.Ratings)
            .FirstOrDefaultAsync(m => m.Id == context.MovieId, context.Ct);

        if (movie is null)
            return false;

        var rating = movie.Ratings.FirstOrDefault(r => r.UserId == context.UserId);

        if (rating is null)
        {
            movie.Ratings.Add(new Rating
            {
                UserId = context.UserId,
                UserRating = context.Rating
            });
        }
        else
        {
            if (rating.UserRating == context.Rating)
                return true;

            rating.UserRating = context.Rating;
        }

        await _dbContext.SaveChangesAsync(context.Ct);
        return true;
    }

    public async Task<bool> DeleteRatingAsync(RatingIdContext context)
    {
        var rating = await _dbContext.Ratings
            .FirstOrDefaultAsync(r => r.MovieId == context.MovieId && r.UserId == context.UserId, context.Ct);

        if (rating is null)
            return false;

        _dbContext.Ratings.Remove(rating);
            await _dbContext.SaveChangesAsync();

        return true;
    }
}
