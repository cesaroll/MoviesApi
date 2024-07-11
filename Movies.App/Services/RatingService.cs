using Microsoft.EntityFrameworkCore;
using Movies.App.Database;
using Movies.App.Models;

namespace Movies.App.Services;

public class RatingService : IRatingService
{
    private readonly MoviesDbContext _context;
    
    public RatingService(MoviesDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> RateMovieAsync(RatingIdContext context)
    {
        var movie = await _context.Movies
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
        
        await _context.SaveChangesAsync(context.Ct);
        return true;
    }
}