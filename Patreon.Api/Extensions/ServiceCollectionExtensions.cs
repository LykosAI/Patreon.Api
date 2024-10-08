using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Patreon.Api.Authorization;
using Patreon.Api.Configs;
using Patreon.Api.Models;
using Refit;

namespace Patreon.Api.Extensions;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Patreon API client and related services to the service collection.
    /// Configures the HttpClient for the Patreon API, sets up the necessary message handlers,
    /// and integrates the Refit library with a custom content serializer.
    /// </summary>
    /// <param name="services">The service collection to which the Patreon API services will be added.</param>
    /// <returns>The configured IHttpClientBuilder for further customization.</returns>
    public static IHttpClientBuilder AddPatreonApi(this IServiceCollection services)
    {
        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                PatreonApiSourceGenerationContext.Default.Options
            ),
        };

        services.AddTransient<PatreonAuthHeaderHandler>();

        return services
            .AddRefitClient<IPatreonApi>(refitSettings)
            .ConfigureHttpClient(
                (provider, client) =>
                {
                    var endpointConfig = provider.GetRequiredService<IOptions<PatreonEndpointConfig>>().Value;
                    client.BaseAddress = endpointConfig.BaseAddress;
                }
            )
            .AddHttpMessageHandler<PatreonAuthHeaderHandler>();
    }
}
