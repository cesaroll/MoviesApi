using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Configs.Auth;
using Movies.Api.Mapping;
using Movies.App.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[Authorize]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMoviesService _moviesService;

    public MoviesController(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create(
        [FromBody] 
        CreateMovieRequest request,
        CancellationToken ct)
    {
        var movie = request.MapToMovie();
        var result = await _moviesService.CreateAsync(new MovieContext(movie, ct));
        
        if (!result)
            return StatusCode((int)HttpStatusCode.Conflict);

        var response = movie.MapToMovieResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get(
        [FromRoute]
        string idOrSlug, 
        CancellationToken ct)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _moviesService.GetByIdAsync(new IdContext(id, ct))
            : await _moviesService.GetBySlugAsync(new SlugContext(idOrSlug, ct));

        if (movie == null)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse();
        return Ok(response);
    }
    
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var movies = await _moviesService.GetAllAsync(ct);
        var response = movies.MapToMoviesResponse();
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
        var movie = request.MapToMovie(id);

        var result = await _moviesService.UpdateAsync(new MovieContext(movie, ct));

        if (result == null)
            return NotFound();

        var response = result.MapToMovieResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete(
        [FromRoute]
        Guid id,
        CancellationToken ct)
    {
        await _moviesService.DeleteByIdAsync(new IdContext(id, ct));
        return Ok();
    }
}
