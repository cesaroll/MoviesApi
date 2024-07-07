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
    
    [Fact]
    public async Task DeleteMovie_Unauthorized_SutClient_ShouldReturn_Unauthorized()
    {
        // Arrange
        var createdMovie = await CreateMovie();
    
        // Act
        var response = await SutClient.DeleteAsync($"/api/movies/{createdMovie!.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteMovie_SutClientWithJwt_ShouldReturn_Forbidden()
    {
        // Arrange
        var createdMovie = await CreateMovie();
    
        // Act
        var response = await SutClientWithJwt.DeleteAsync($"/api/movies/{createdMovie!.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task DeleteMovie_NonAdminJwt_ShouldReturn_Forbidden()
    {
        // Arrange
        var createdMovie = await CreateMovie();
    
        // Act
        var response = await SutClientWithJwtTrustedMember.DeleteAsync($"/api/movies/{createdMovie!.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}