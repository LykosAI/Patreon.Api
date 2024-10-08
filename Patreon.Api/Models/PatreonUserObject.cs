using System.Text.Json.Serialization;

namespace Patreon.Api.Models;

public class PatreonUserObject
{
    [JsonPropertyName("data")]
    public PatreonDataObject? Data { get; set; }
}
