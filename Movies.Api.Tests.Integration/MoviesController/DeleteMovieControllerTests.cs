using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesController;

[Collection("MoviesApi Collection")]
public class DeleteMovieControllerTests
{
    private HttpClient _client;
    
    public DeleteMovieControllerTests(MoviesApiFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task DeleteExistingMovie_ShouldReturn_Success()
    {
        // Arrange
        var createdMovie = await CreateMovie();

        // Act
        var response = await _client.DeleteAsync($"/api/movies/{createdMovie!.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteNonExistingMovie_ShouldReturn_Success()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/movies/{Guid.NewGuid()}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    private async Task<MovieResponse?> CreateMovie()
    {
        var createMovieRequest = CreateMovieRequestBuilder.CreateOne();
        
        var createdResponse = await _client.PostAsJsonAsync("/api/movies", createMovieRequest);
        return await createdResponse.Content.ReadFromJsonAsync<MovieResponse>();
    }
}