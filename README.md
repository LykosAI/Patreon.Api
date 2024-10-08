## Patreon Api
[![NuGet Version](https://img.shields.io/nuget/v/Patreon.Api)](https://www.nuget.org/packages/Patreon.Api)
[![Build](https://github.com/LykosAI/Patreon.Api/actions/workflows/build.yml/badge.svg)](https://github.com/LykosAI/Patreon.Api/actions/workflows/build.yml)
![GitHub License](https://img.shields.io/github/license/LykosAI/Patreon.Api)

A minimal .NET Client for [Patreon API v2](https://docs.patreon.com/) using Refit and Polly. Supports automatic refresh of expired access tokens.

## Usage
### Setup with Dependency Injection
```csharp
using Patreon.Api.Extensions;

var services = new ServiceCollection();

services.AddPatreonApi(options =>
{
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
});
```

### Inject the IPatreonApi Polly Client to use
```csharp
using Patreon.Api;

public class MyService(IPatreonApi patreonApi)
{
    // Get the active tier ids for a user based on your campaign id
    public async Task GetTiers()
    {
        AuthorizationTokens tokens = new()
        {
            AccessToken = "your-user-access-token",
            RefreshToken = "your-user-refresh-token"
        };
        
        PatreonIdentityResponse response = await patreonApi.GetIdentity(
            include: "memberships.campaign,memberships.currently_entitled_tiers"
            authorization: tokens
        );
        
        // If automatic token refresh occured, the `tokens` object will have updated properties with the new tokens
        // - tokens.AccessToken
        // - tokens.AccessTokenExpiration
        // - tokens.RefreshToken
        
        IReadOnlyList<string>? tiers = response.GetTierIds("your-campaign-id");
    }
    
    // Manually refreshing the access + refresh tokens using a user refresh token and your client id + secret
    public async Task ManualTokenRefresh()
    {        
        PatreonTokenResponse response = await patreonApi.GetToken(
            PatreonTokenRequest.FromRefreshToken(
                authTokens.RefreshToken,
                "your-client-id",
                "your-client-secret"
            )
        );
    }
    
    // Getting access + refresh tokens using a code from the Patreon OAuth2 flow
    public async Task GetTokensFromCode(string code)
    {
        PatreonTokenResponse response = await patreonApi.GetToken(
            PatreonTokenRequest.FromCode(
                code,
                "your-client-id",
                "your-client-secret",
                "your-redirect-uri"
            )
        );
    }
}
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
This project is licensed under the Apache 2.0 License - see the [LICENSE](LICENSE) file for details.

## Disclaimers
All trademarks, logos, and brand names are the property of their respective owners. All company, product and service names used in this document and licensed applications are for identification purposes only. Use of these names, trademarks, and brands does not imply endorsement.
