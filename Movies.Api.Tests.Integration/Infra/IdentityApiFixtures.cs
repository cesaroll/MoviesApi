using System.Text;

namespace Movies.Api.Tests.Integration.Infra;

public static class IdentityApiFixtures
{
    public static async Task<string> GetAdminJwtAsync(this IdentityApiFactory factory)
    {
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "/token");
        
        request.Content = new StringContent(
         @"{
                 ""userId"": ""de3f7d8b-b237-4369-8afe-e4b3c380d135"",
                 ""email"": ""cesar@ces.com"",
                 ""customClaims"": {
                     ""admin"": true,
                     ""trusted_member"": true
                 }
             }",
         Encoding.UTF8,
         "application/json");

        var response = await client.SendAsync(request);
        var jwt = await response.Content.ReadAsStringAsync();
        return jwt;
    }
}