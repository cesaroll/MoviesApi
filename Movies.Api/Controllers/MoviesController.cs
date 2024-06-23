using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.App.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMoviesRepository _moviesRepository;

    public MoviesController(IMoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        var result = await _moviesRepository.CreateAsync(movie);
        
        var response = movie.MapToMovieResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }
    
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute]string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _moviesRepository.GetByIdAsync(id)
            : await _moviesRepository.GetBySlugAsync(idOrSlug);
        
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
        var movies = await _moviesRepository.GetAllAsync();
        var response = movies.MapToMoviesResponse();
        return Ok(response);
    }
    
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateMovieRequest request)
    {
        if (!await _moviesRepository.ExistsByIdAsync(id))
            return NotFound();
        
        var movie = request.MapToMovie(id);
        
        await _moviesRepository.UpdateAsync(movie);
        
        var response = movie.MapToMovieResponse();
        return Ok(response);
    }
    
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute]Guid id)
    {
        await _moviesRepository.DeleteByIdAsync(id);
        return Ok();
    }
}