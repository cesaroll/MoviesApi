using System.Net.Http.Json;
using FluentAssertions;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesController;

[Collection("MoviesApi Collection")]
public class ReadMovieControllerTests // : IClassFixture<MoviesApiFactory>
{
    private HttpClient _client;
    public ReadMovieControllerTests(MoviesApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Movie()
    {
        // Arrange
        var createMovieRequest = CreateMovieRequestBuilder.CreateOne();
        
        var createdResponse = await _client.PostAsJsonAsync("/api/movies", createMovieRequest);
        var createdMovie = await createdResponse.Content.ReadFromJsonAsync<MovieResponse>();
        
        // Act
        var readResponse = await _client.GetAsync($"/api/movies/{createdMovie!.Id}");
        
        // Assert
        readResponse.EnsureSuccessStatusCode();
        var movieResponse = await readResponse.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        movieResponse!.Id.Should().Be(createdMovie.Id);
    }
}