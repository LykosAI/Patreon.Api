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
    /// <param name="setupAction">An optional action to configure the Patreon client settings.</param>
    /// <returns>The configured IHttpClientBuilder for further customization.</returns>
    public static IHttpClientBuilder AddPatreonApi(
        this IServiceCollection services,
        Action<PatreonClientConfig>? setupAction = null
    )
    {
        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                PatreonApiSourceGenerationContext.Default.Options
            ),
        };

        services.AddTransient<PatreonAuthHeaderHandler>();

        var clientBuilder = services
            .AddRefitClient<IPatreonApi>(refitSettings)
            .ConfigureHttpClient(
                (provider, client) =>
                {
                    var endpointConfig = provider.GetRequiredService<IOptions<PatreonEndpointConfig>>().Value;
                    client.BaseAddress = endpointConfig.BaseAddress;
                }
            )
            .AddHttpMessageHandler<PatreonAuthHeaderHandler>();

        if (setupAction != null)
        {
            services.ConfigurePatreonApi(setupAction);
        }

        return clientBuilder;
    }

    /// <summary>
    /// Configures the Patreon API client settings using the specified setup action.
    /// </summary>
    /// <param name="services">The service collection to which the Patreon API configuration will be added.</param>
    /// <param name="setupAction">An action to configure the Patreon client settings.</param>
    public static void ConfigurePatreonApi(
        this IServiceCollection services,
        Action<PatreonClientConfig> setupAction
    )
    {
        services.Configure(setupAction);
    }
}
