using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Configs;
using Movies.Api.Configs.Auth;
using Movies.App.Services;

namespace Movies.Api.Controllers;

[Authorize]
[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Movies.Rate)]
    public async Task<IActionResult> Rate(
        [FromRoute]
        Guid id,
        [FromRoute]
        int rating,
        CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        var result = await _ratingService.RateMovieAsync(new RatingIdContext(id, userId!.Value, rating, ct));

        return result ? Ok() : NotFound();
    }

    [Authorize]
    [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
    public async Task<IActionResult> DeleteRating(
        [FromRoute]
        Guid id,
        CancellationToken ct
    )
    {
        var userId = HttpContext.GetUserId();

        var result = await _ratingService.DeleteRatingAsync(new RatingIdContext(id, userId!.Value, 0, ct));

        return result ? Ok() : NotFound();
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
    public async Task<IActionResult> GetAllUserRatings(CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        var ratings = await _ratingService.GetUserRatings(userId!.Value, ct);

        var response = ratings.MapToMovieRatingResponse();
        return Ok(response);
    }
}
