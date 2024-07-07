using System.Net.Http.Headers;

namespace Movies.Api.Tests.Integration.Infra;

public static class MoviesApiFixtures
{
    private static async Task<HttpClient> CreateClientJwtAsync(
        this MoviesApiFactory moviesApiFactory, 
        IdentityApiFactory identityApiFactory,
        bool isAdmin = false,
        bool isTrustedMember = false)
    {
        var moviesApiHttpClient = moviesApiFactory.CreateClient();
        var jwt = await identityApiFactory.GetJwtAsync(isAdmin, isTrustedMember);
        moviesApiHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return moviesApiHttpClient;
    }
    
    public static async Task<HttpClient> CreateClientWithRegularJwtAsync(this MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory) =>
        await moviesApiFactory.CreateClientJwtAsync(identityApiFactory, isAdmin: false, isTrustedMember: false);
    
    public static async Task<HttpClient> CreateClientWithTrustedMemberJwtAsync(this MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory) =>
        await moviesApiFactory.CreateClientJwtAsync(identityApiFactory, isAdmin: false, isTrustedMember: true);
    
    public static async Task<HttpClient> CreateClientWithAdminJwtAsync(this MoviesApiFactory moviesApiFactory, IdentityApiFactory identityApiFactory) =>
        await moviesApiFactory.CreateClientJwtAsync(identityApiFactory, isAdmin: true, isTrustedMember: true);
}