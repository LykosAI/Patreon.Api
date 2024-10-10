using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Patreon.Api.Models;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(PatreonIdentityResponse))]
[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(JsonValue))]
internal partial class PatreonApiSourceGenerationContext : JsonSerializerContext;
