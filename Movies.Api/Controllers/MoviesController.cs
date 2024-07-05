using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.App.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMoviesService _moviesService;

    public MoviesController(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        var result = await _moviesService.CreateAsync(movie);

        var response = movie.MapToMovieResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [HttpGet("ping")]
    public IActionResult GetPing()
    {
        return Ok("pong");
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute]string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _moviesService.GetByIdAsync(id)
            : await _moviesService.GetBySlugAsync(idOrSlug);

        if (movie == null)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _moviesService.GetAllAsync();
        var response = movies.MapToMoviesResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);

        var result = await _moviesService.UpdateAsync(movie);

        if (result == null)
            return NotFound();

        var response = result.MapToMovieResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute]Guid id)
    {
        await _moviesService.DeleteByIdAsync(id);
        return Ok();
    }
}
