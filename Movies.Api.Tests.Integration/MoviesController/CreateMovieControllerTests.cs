using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Api.Tests.Integration.Infra;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesController;

public class CreateMovieControllerTests : MovieControllerTests
{
    public CreateMovieControllerTests(MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory) : base(moviesApiFactory, identityApiFactory)
    {
    }
    
    [Fact]
    public async Task CreateMovie_ShouldReturn_Success()
    {
        // Arrange
        var movieRequest = CreateMovieRequestBuilder.Build()
            .WithTitle("Titanic")
            .WithYearOfRelease(1997)
            .WithGenre("Drama")
            .WithGenre("Romance")
            .Create();

        // Act
        var response = await SutClientWithJwtAdmin.PostAsJsonAsync("/api/movies", movieRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Should().Contain(h => h.Key == "Location");
        var location = response.Headers.FirstOrDefault(h => h.Key == "Location").Value.FirstOrDefault();
        location.Should().NotBeNullOrEmpty();
        
        var movieResponse = await response.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        
        location.Should().Be($"{SutClientWithJwtAdmin.BaseAddress}api/movies/{movieResponse?.Id}");
        
        movieResponse!.Title.Should().Be(movieRequest.Title);
        movieResponse.YearOfRelease.Should().Be(movieRequest.YearOfRelease);
        movieResponse.Genres.Should().BeEquivalentTo(movieRequest.Genres);
    }
    
    [Fact]
    public async Task CreateMovie_json_ShouldReturn_Success()
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
        var response = await SutClientWithJwtAdmin.SendAsync(request);
    
        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Should().Contain(h => h.Key == "Location");
        var location = response.Headers.FirstOrDefault(h => h.Key == "Location").Value.FirstOrDefault();
        location.Should().NotBeNullOrEmpty();
        
        var movieResponse = await response.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        
        location.Should().Be($"{SutClientWithJwtAdmin.BaseAddress}api/movies/{movieResponse?.Id}");
    }
    
    [Fact]
    public async Task CreateMovie_InvalidRequest_ShouldReturn_BadRequest()
    {
        // Arrange
        var movieRequest = CreateMovieRequestBuilder.Build()
            .WithTitle(string.Empty)
            .WithYearOfRelease(1800)
            .Create();
    
        // Act
        var response = await SutClientWithJwtAdmin.PostAsJsonAsync("/api/movies", movieRequest);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateMovie_ExistingMovie_ShouldReturn_Conflict()
    {
        // Arrange
        var movieRequest = CreateMovieRequestBuilder.CreateOne();
    
        await SutClientWithJwtAdmin.PostAsJsonAsync("/api/movies", movieRequest);
    
        // Act
        var response = await SutClientWithJwtAdmin.PostAsJsonAsync("/api/movies", movieRequest);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async Task CreateMovie_SutClient_ShouldReturn_Unauthorized()
    {
        // Arrange
        var movieRequest = CreateMovieRequestBuilder.CreateOne();
    
        // Act
        var response = await SutClient.PostAsJsonAsync("/api/movies", movieRequest);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task CreateMovie_SutClientWithJwt_ShouldReturn_Forbidden()
    {
        // Arrange
        var movieRequest = CreateMovieRequestBuilder.CreateOne();
    
        // Act
        var response = await SutClientWithJwt.PostAsJsonAsync("/api/movies", movieRequest);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
