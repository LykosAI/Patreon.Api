using System.Text.Json.Serialization;

namespace Patreon.Api.Models;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(PatreonIdentityResponse))]
internal partial class PatreonApiSourceGenerationContext : JsonSerializerContext;
