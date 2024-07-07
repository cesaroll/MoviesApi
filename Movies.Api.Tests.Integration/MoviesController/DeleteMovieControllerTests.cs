using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Api.Tests.Integration.Infra;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesController;

public class DeleteMovieControllerTests : MovieControllerTests
{
    public DeleteMovieControllerTests(MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory) : base(moviesApiFactory, identityApiFactory)
    {
    }
    
    [Fact]
    public async Task DeleteExistingMovie_ShouldReturn_Success()
    {
        // Arrange
        var createdMovie = await CreateMovie();

        // Act
        var response = await SutClientWithJwtAdmin.DeleteAsync($"/api/movies/{createdMovie!.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteNonExistingMovie_ShouldReturn_Success()
    {
        // Act
        var response = await SutClientWithJwtAdmin.DeleteAsync($"/api/movies/{Guid.NewGuid()}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}