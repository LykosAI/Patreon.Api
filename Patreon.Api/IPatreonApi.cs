using System.Text.Json.Nodes;
using JetBrains.Annotations;
using Patreon.Api.Authorization;
using Patreon.Api.Models;
using Refit;

namespace Patreon.Api;

[PublicAPI]
public interface IPatreonApi
{
    private const string DefaultInclude = "memberships.campaign,memberships.currently_entitled_tiers";
    private const string DefaultUserFields =
        "email,first_name,full_name,image_url,last_name,thumb_url,url,vanity,is_email_verified";
    private const string DefaultMemberFields =
        "currently_entitled_amount_cents,lifetime_support_cents,campaign_lifetime_support_cents,last_charge_status,patron_status,last_charge_date,pledge_relationship_start,pledge_cadence";

    [Headers("Content-Type: application/x-www-form-urlencoded")]
    [Post("/oauth2/token")]
    Task<PatreonTokenResponse> GetToken(
        [Query] PatreonTokenRequest request,
        CancellationToken cancellationToken = default
    );

    [Get("/oauth2/v2/identity")]
    Task<PatreonIdentityResponse> GetIdentity(
        string? include = DefaultInclude,
        [AliasAs("fields[user]")] string? userFields = DefaultUserFields,
        [AliasAs("fields[member]")] string? memberFields = DefaultMemberFields,
        [Property(nameof(AuthorizationTokens))] AuthorizationTokens? authorization = null,
        CancellationToken cancellationToken = default
    );

    [Get("/oauth2/v2/identity")]
    Task<JsonObject> GetIdentityJson(
        string? include = DefaultInclude,
        [AliasAs("fields[user]")] string? userFields = DefaultUserFields,
        [AliasAs("fields[member]")] string? memberFields = DefaultMemberFields,
        [Property(nameof(AuthorizationTokens))] AuthorizationTokens? authorization = null,
        CancellationToken cancellationToken = default
    );
}
