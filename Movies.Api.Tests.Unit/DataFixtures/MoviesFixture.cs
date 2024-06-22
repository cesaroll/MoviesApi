using AutoFixture;
using Movies.App.Models;

namespace Movies.Api.Tests.Unit.DataFixtures;

public class MoviesFixture
{
    private readonly IFixture _fixture;

    public MoviesFixture()
    {
        _fixture = new Fixture();
    }
    
    public IFixture Fixture => _fixture;
    
    public Movie GetMovie()
    {
        var movie = _fixture.Build<Movie>()
            .Without(m => m.Genres)
            .Create();

        movie = WithGenres(movie);

        return movie;
    }

    public List<Movie> GetMovies()
    {
        var movies = _fixture.Build<Movie>()
            .Without(m => m.Genres)
            .CreateMany()
            .ToList();

        movies = movies.Select(WithGenres).ToList();

        return movies;
    }
    
    private Movie WithGenres(Movie movie)
    {
        var genres = _fixture.Build<Genre>()
            .With(g => g.MovieId, movie.Id)
            .With(g => g.Movie, movie)
            .CreateMany()
            .ToList();
        
        movie = _fixture.Build<Movie>()
            .With(m => m.Id, movie.Id)
            .With(m => m.Genres, genres)
            .Create();

        return movie;
    }
}