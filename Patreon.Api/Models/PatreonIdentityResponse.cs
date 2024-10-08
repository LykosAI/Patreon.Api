using System.Text.Json.Serialization;

namespace Patreon.Api.Models;

public class PatreonIdentityResponse
{
    [JsonPropertyName("data")]
    public required PatreonDataObject Data { get; set; }

    [JsonPropertyName("included")]
    public List<PatreonIncludeObject>? Included { get; set; }
}
