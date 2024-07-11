namespace Movies.Api.Configs.Auth;

public static class IdentityExtensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var userId = context.User.Claims.SingleOrDefault(x => x.Type == "userId");
        
        return Guid.TryParse(userId?.Value, out var parsedId)
            ? parsedId
            : null;
    }
}