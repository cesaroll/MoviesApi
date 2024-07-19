using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Configs;
using Movies.Api.Configs.Auth;
using Movies.Api.Mapping;
using Movies.App.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[Authorize]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IRatingService _ratingService;

    public MoviesController(IMovieService movieService, IRatingService ratingService)
    {
        _movieService = movieService;
        _ratingService = ratingService;
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create(
        [FromBody]
        CreateMovieRequest request,
        CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();
        var movie = request.MapToMovie();

        var result = await _movieService.CreateAsync(new MovieContext(movie, userId, ct));

        if (!result)
            return StatusCode((int)HttpStatusCode.Conflict);

        var response = movie.MapToMovieResponse(userId);
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get(
        [FromRoute]
        string idOrSlug,
        CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(new MovieIdContext(id, userId, ct))
            : await _movieService.GetBySlugAsync(new SlugContext(idOrSlug, userId, ct));

        if (movie == null)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse(userId);
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        var movies = await _movieService.GetAllAsync(ct);

        var response = movies.MapToMoviesResponse(userId);
        return Ok(response);
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update(
        [FromRoute]
        Guid id,
        [FromBody]
        UpdateMovieRequest request,
        CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();
        var movie = request.MapToMovie(id);

        var result = await _movieService.UpdateAsync(new MovieContext(movie, userId, ct));

        if (result == null)
            return NotFound();

        var response = result.MapToMovieResponse(userId);
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete(
        [FromRoute]
        Guid id,
        CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        await _movieService.DeleteByIdAsync(new MovieIdContext(id, userId, ct));
        return Ok();
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
}
