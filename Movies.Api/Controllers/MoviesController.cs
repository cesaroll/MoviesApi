using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappers;
using Movies.App.Models;
using Movies.App.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
[Route("api")]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }

    [HttpPost("movies")]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.ToMovie();
        var result = await _movieRepository.CreateAsync(movie);
        
        var movieResponse = movie.ToMovieResponse();
        return Created($"/api/movies/{movie.Id}", movieResponse);
    }
}