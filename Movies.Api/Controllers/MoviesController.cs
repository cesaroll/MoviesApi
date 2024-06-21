using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.App.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        var result = await _movieRepository.CreateAsync(movie);
        
        var movieResponse = movie.MapToMovieResponse();
        return Created($"{ApiEndpoints.Movies.Create}/{movie.Id}", movieResponse);
    }
}