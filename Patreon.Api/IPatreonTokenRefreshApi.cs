using Patreon.Api.Models;
using Refit;

namespace Patreon.Api;

internal interface IPatreonTokenRefreshApi
{
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    [Post("/oauth2/token")]
    Task<PatreonTokenResponse> GetToken(
        [Query] PatreonTokenRequest request,
        CancellationToken cancellationToken = default
    );
}
