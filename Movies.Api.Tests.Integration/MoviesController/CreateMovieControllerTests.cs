using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesController;

public class CreateMovieControllerTests :  IClassFixture<MoviesApiFactory>
{
    private HttpClient _client;

    public CreateMovieControllerTests(MoviesApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }
    
    [Fact]
    public async Task CreateMovie_Returns_Success_json()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/movies");
        
        request.Content = new StringContent(
            @"{
                ""title"": ""The Matrix"",
                ""yearOfRelease"": 1999,
                ""genres"": [""Action""]
            }",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Should().Contain(h => h.Key == "Location");
        var location = response.Headers.FirstOrDefault(h => h.Key == "Location").Value.FirstOrDefault();
        location.Should().NotBeNullOrEmpty();
        
        var movieResponse = await response.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        
        location.Should().Be($"{_client.BaseAddress}api/movies/{movieResponse?.Id}");
    }
    
    [Fact]
    public async Task CreateMovie_Returns_Success()
    {
        // Arrange
        var movieRequest = CreateMovieRequestBuilder.Build()
            .WithTitle("Titanic")
            .WithYearOfRelease(1997)
            .WithGenre("Drama")
            .WithGenre("Romance")
            .Create();

        // Act
        var response = await _client.PostAsJsonAsync("/api/movies", movieRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Should().Contain(h => h.Key == "Location");
        var location = response.Headers.FirstOrDefault(h => h.Key == "Location").Value.FirstOrDefault();
        location.Should().NotBeNullOrEmpty();
        
        var movieResponse = await response.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        
        location.Should().Be($"{_client.BaseAddress}api/movies/{movieResponse?.Id}");
        
        movieResponse!.Title.Should().Be(movieRequest.Title);
        movieResponse.YearOfRelease.Should().Be(movieRequest.YearOfRelease);
        movieResponse.Genres.Should().BeEquivalentTo(movieRequest.Genres);
    }
}
