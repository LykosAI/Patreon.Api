using Refit;

namespace Patreon.Api.Models;

public class PatreonTokenRequest
{
    [AliasAs("grant_type")]
    public required string GrantType { get; set; }

    [AliasAs("code")]
    public string? Code { get; set; }

    [AliasAs("refresh_token")]
    public string? RefreshToken { get; set; }

    [AliasAs("client_id")]
    public required string ClientId { get; set; }

    [AliasAs("client_secret")]
    public required string ClientSecret { get; set; }

    [AliasAs("redirect_uri")]
    public string? RedirectUri { get; set; }

    public static PatreonTokenRequest FromCode(
        string code,
        string clientId,
        string clientSecret,
        string redirectUri
    )
    {
        return new PatreonTokenRequest
        {
            GrantType = "authorization_code",
            Code = code,
            ClientId = clientId,
            ClientSecret = clientSecret,
            RedirectUri = redirectUri,
        };
    }

    public static PatreonTokenRequest FromRefreshToken(
        string refreshToken,
        string clientId,
        string clientSecret
    )
    {
        return new PatreonTokenRequest
        {
            GrantType = "refresh_token",
            RefreshToken = refreshToken,
            ClientId = clientId,
            ClientSecret = clientSecret,
        };
    }
}
