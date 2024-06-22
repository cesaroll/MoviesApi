using System.Net.Http.Json;
using FluentAssertions;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesController;

public class ReadMovieControllerTests : IClassFixture<MoviesApiFactory>
{
    private readonly MoviesApiFactory _apiFactory;

    public ReadMovieControllerTests(MoviesApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Movie()
    {
        // Arrange
        var createMovieRequest = CreateMovieRequestBuilder.CreateOne();
        var client = _apiFactory.CreateClient();
        
        var createdResponse = await client.PostAsJsonAsync("/api/movies", createMovieRequest);
        var createdMovie = await createdResponse.Content.ReadFromJsonAsync<MovieResponse>();
        
        // Act
        var readResponse = await client.GetAsync($"/api/movies/{createdMovie!.Id}");
        
        // Assert
        readResponse.EnsureSuccessStatusCode();
        var movieResponse = await readResponse.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        movieResponse!.Id.Should().Be(createdMovie.Id);
    }
}