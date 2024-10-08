using System.Text.Json.Serialization;

namespace Patreon.Api.Models;

public class PatreonRelationshipObject
{
    [JsonPropertyName("campaign")]
    public PatreonCampaignObject? Campaign { get; set; }

    [JsonPropertyName("currently_entitled_tiers")]
    public PatreonTiersObject? CurrentlyEntitledTiers { get; set; }

    [JsonPropertyName("user")]
    public PatreonUserObject? User { get; set; }
}
