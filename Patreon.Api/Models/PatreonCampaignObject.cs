using System.Text.Json.Serialization;

namespace Patreon.Api.Models;

public class PatreonCampaignObject
{
    [JsonPropertyName("data")]
    public required PatreonDataObject Data { get; set; }
}
