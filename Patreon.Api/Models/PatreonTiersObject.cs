using System.Text.Json.Serialization;

namespace Patreon.Api.Models;

public class PatreonTiersObject
{
    [JsonPropertyName("data")]
    public required List<PatreonDataObject> Data { get; set; }
}
