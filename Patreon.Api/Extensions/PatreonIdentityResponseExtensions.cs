using System.Collections.Immutable;
using JetBrains.Annotations;
using Patreon.Api.Models;

namespace Patreon.Api.Extensions;

[PublicAPI]
public static class PatreonIdentityResponseExtensions
{
    public static IReadOnlyList<string>? GetTierIds(this PatreonIdentityResponse response, string campaignId)
    {
        ArgumentNullException.ThrowIfNull(campaignId);

        return response
            .Included?.Where(x => x.Relationships?.Campaign?.Data.Id == campaignId)
            .Select(x => x.Relationships?.CurrentlyEntitledTiers)
            .Where(tiers => tiers != null)
            .SelectMany(x => x!.Data)
            .Where(data => data.Type == "tier")
            .Select(data => data.Id)
            .ToImmutableArray();
    }

    public static IReadOnlyList<string>? GetWebhookTierIds(
        this PatreonIdentityResponse response,
        string campaignId
    )
    {
        ArgumentNullException.ThrowIfNull(campaignId);

        if (response.Data.Relationships?.Campaign?.Data.Id != campaignId)
            return [];

        return response
            .Data.Relationships?.CurrentlyEntitledTiers?.Data.Where(d => d is { Type: "tier", Id: not null })
            .Select(tier => tier.Id!)
            .ToImmutableArray();
    }
}
