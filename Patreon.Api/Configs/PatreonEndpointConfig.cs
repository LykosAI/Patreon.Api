namespace Patreon.Api.Configs;

public class PatreonEndpointConfig
{
    public Uri? BaseAddress { get; set; } = new Uri("https://www.patreon.com/api");
}
