using System.Net.Http.Json;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Api.Tests.Integration.Infra;
using Movies.Contracts.Responses;

namespace Movies.Api.Tests.Integration.MoviesControllerTests;

[Collection("MoviesApi Collection")]
public abstract class MovieControllerTests : IClassFixture<IdentityApiFactory>, IAsyncLifetime
{
    protected HttpClient SutClient;
    protected HttpClient SutClientWithJwt;
    protected HttpClient SutClientWithJwtTrustedMember;
    protected HttpClient SutClientWithJwtAdmin;
    
    protected readonly MoviesApiFactory MoviesApiFactory;
    protected readonly IdentityApiFactory IdentityApiFactory;

    public MovieControllerTests(MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory)
    {
        MoviesApiFactory = moviesApiFactory;
        IdentityApiFactory = identityApiFactory;
    }

    public virtual async Task InitializeAsync()
    {
        SutClient = MoviesApiFactory.CreateClient();
        SutClientWithJwt = await MoviesApiFactory.CreateClientWithRegularJwtAsync(IdentityApiFactory);
        SutClientWithJwtTrustedMember = await MoviesApiFactory.CreateClientWithTrustedMemberJwtAsync(IdentityApiFactory);
        SutClientWithJwtAdmin = await MoviesApiFactory.CreateClientWithAdminJwtAsync(IdentityApiFactory);
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;
    
    protected async Task<MovieResponse?> CreateMovie()
    {
        var createMovieRequest = CreateMovieRequestBuilder.CreateOne();
        
        var createdResponse = await SutClientWithJwtAdmin.PostAsJsonAsync("/api/movies", createMovieRequest);
        return await createdResponse.Content.ReadFromJsonAsync<MovieResponse>();
    }
}