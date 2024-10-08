using JetBrains.Annotations;
using Patreon.Api.Authorization;
using Patreon.Api.Models;
using Refit;

namespace Patreon.Api;

[PublicAPI]
public interface IPatreonApi
{
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    [Post("/oauth2/token")]
    Task<PatreonTokenResponse> GetToken(
        [Query] PatreonTokenRequest request,
        CancellationToken cancellationToken = default
    );

    [Get("/oauth2/v2/identity")]
    Task<PatreonIdentityResponse> GetIdentity(
        [Query] string include = "memberships.campaign,memberships.currently_entitled_tiers",
        [Property] AuthorizationTokens? authorization = null,
        CancellationToken cancellationToken = default
    );
}
