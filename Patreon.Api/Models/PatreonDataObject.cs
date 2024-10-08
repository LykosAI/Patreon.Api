using System.Text.Json.Serialization;

namespace Patreon.Api.Models;

public class PatreonDataObject
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("relationships")]
    public PatreonRelationshipObject? Relationships { get; set; }
}
