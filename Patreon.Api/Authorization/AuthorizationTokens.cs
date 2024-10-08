namespace Patreon.Api.Authorization;

public class AuthorizationTokens
{
    public string? AccessToken { get; set; }

    public DateTimeOffset? AccessTokenExpiration { get; set; }

    public string? RefreshToken { get; set; }
}
