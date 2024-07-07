using System.Text;

namespace Movies.Api.Tests.Integration.Infra;

public static class IdentityApiFixtures
{
    public static async Task<string> GetJwtAsync(
        this IdentityApiFactory factory, bool isAdmin = false, bool isTrustedMember = false)
    {
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "/token");

        var stringContent = @$"{{
            ""userId"": ""de3f7d8b-b237-4369-8afe-e4b3c380d135"",
            ""email"": ""cesar@ces.com"",
            ""customClaims"": {{
                ""admin"": {isAdmin.ToString().ToLower()},
                ""trusted_member"": {isTrustedMember.ToString().ToLower()}
                }}
        }}";
        
        request.Content = new StringContent(
            stringContent,
            Encoding.UTF8,
            "application/json");

        var response = await client.SendAsync(request);
        var jwt = await response.Content.ReadAsStringAsync();
        return jwt;
    }
}