using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Api.Tests.Integration.Infra;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesControllerTests;

public class UpdateMovieControllerTests : MovieControllerTests
{
    public UpdateMovieControllerTests(MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory) : base(moviesApiFactory, identityApiFactory)
    {
    }

    [Fact]
    public async Task UpdateMovie_ShouldReturn_Success()
    {
        // Arrange
        var createdMovie = await CreateMovie();

        var movieRequest = UpdateMovieRequestBuilder.CreateOne();

        // Act
        var response = await SutClientWithJwtAdmin.PutAsJsonAsync($"/api/movies/{createdMovie!.Id}", movieRequest);
        
        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var movieResponse = await response.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();
        
        movieResponse!.Id.Should().Be(createdMovie.Id);
        movieResponse.Title.Should().Be(movieRequest.Title);
        movieResponse.YearOfRelease.Should().Be(movieRequest.YearOfRelease);
        movieResponse.Genres.Should().BeEquivalentTo(movieRequest.Genres);
    }
    
    [Fact]
    public async Task UpdateMovie_ShouldReturn_NotFound()
    {
        // Arrange
        var movieRequest = UpdateMovieRequestBuilder.CreateOne();
        
        // Act
        var response = await SutClientWithJwtAdmin.PutAsJsonAsync($"/api/movies/{Guid.NewGuid()}", movieRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task UpdateMovie_SutClient_ShouldReturn_Unauthorized()
    {
        // Arrange
        var movieRequest = UpdateMovieRequestBuilder.CreateOne();
    
        // Act
        var response = await SutClient.PutAsJsonAsync($"/api/movies/{Guid.NewGuid()}", movieRequest);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task UpdateMovie_SutClientWithJwt_ShouldReturn_Forbidden()
    {
        // Arrange
        var movieRequest = UpdateMovieRequestBuilder.CreateOne();
    
        // Act
        var response = await SutClientWithJwt.PutAsJsonAsync($"/api/movies/{Guid.NewGuid()}", movieRequest);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}