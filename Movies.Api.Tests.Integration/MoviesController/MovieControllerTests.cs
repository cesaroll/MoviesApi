using Movies.Api.Tests.Integration.Infra;

namespace Movies.Api.Tests.Integration.MoviesController;

[Collection("MoviesApi Collection")]
public abstract class MovieControllerTests : IClassFixture<IdentityApiFactory>, IAsyncLifetime
{
    protected HttpClient SutAdminClient;
    
    protected readonly MoviesApiFactory MoviesApiFactory;
    protected readonly IdentityApiFactory IdentityApiFactory;

    public MovieControllerTests(MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory)
    {
        MoviesApiFactory = moviesApiFactory;
        IdentityApiFactory = identityApiFactory;
    }

    public virtual async Task InitializeAsync()
    {
        SutAdminClient = await MoviesApiFactory.CreateAdminClientAsync(IdentityApiFactory);
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;
}