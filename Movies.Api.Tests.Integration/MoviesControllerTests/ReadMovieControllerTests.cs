using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Movies.Api.Tests.Integration.Infra;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesControllerTests;

public class ReadMovieControllerTests : MovieControllerTests
{
    public ReadMovieControllerTests(MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory) : base(moviesApiFactory, identityApiFactory)
    {
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Movie()
    {
        // Arrange
        var createdMovie = await CreateMovie();
        
        // Act
        var readResponse = await SutClientWithJwt.GetAsync($"/api/movies/{createdMovie!.Id}");
        
        // Assert
        readResponse.EnsureSuccessStatusCode();
        var movieResponse = await readResponse.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        movieResponse!.Id.Should().Be(createdMovie.Id);
    }

    [Fact]
    public async Task GetMovieBySlug_ShouldReturn_Movie()
    {
        // Arrange
        var createdMovie = await CreateMovie();
        
        // Act
        var readResponse = await SutClientWithJwt.GetAsync($"/api/movies/{createdMovie!.Slug}");
        
        // Assert
        readResponse.EnsureSuccessStatusCode();
        var movieResponse = await readResponse.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        movieResponse!.Id.Should().Be(createdMovie.Id);
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_NotFound()
    {
        // Act
        var readResponse = await SutClientWithJwt.GetAsync($"/api/movies/{Guid.NewGuid()}");
        
        // Assert
        readResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetAllMovies_ShouldReturn_Movies()
    {
        // Arrange
        var createdMovie = await CreateMovie();
        
        // Act
        var readResponse = await SutClientWithJwt.GetAsync($"/api/movies");
        
        // Assert
        readResponse.EnsureSuccessStatusCode();
        var moviesResponse = await readResponse.Content.ReadFromJsonAsync<MoviesResponse>();
        moviesResponse.Should().NotBeNull();
        moviesResponse!.Items.Count().Should().BeGreaterThan(0);
        var list = moviesResponse!.Items.ToList();
        var movieResponse = list.Find(x => x.Id == createdMovie!.Id);
        movieResponse.Should().NotBeNull();
    }
}