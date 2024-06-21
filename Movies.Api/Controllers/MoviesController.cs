using Microsoft.AspNetCore.Mvc;
using Movies.App.Repositories;

namespace Movies.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    [HttpGet("/ping")]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }
}