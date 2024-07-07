using System.Net.Http.Headers;

namespace Movies.Api.Tests.Integration.Infra;

public static class MoviesApiFixtures
{
    public static async Task<HttpClient> CreateAdminClientAsync(this MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory)
    {
        var moviesApiHttpClient = moviesApiFactory.CreateClient();
        var jwt  = await identityApiFactory.GetAdminJwtAsync();
        moviesApiHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return moviesApiHttpClient;
    }
}