using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Patreon.Api.Configs;
using Patreon.Api.Models;
using Polly;
using Polly.Retry;
using Refit;

namespace Patreon.Api.Authorization;

public class PatreonAuthHeaderHandler : DelegatingHandler
{
    private static HttpRequestOptionsKey<AuthorizationTokens> AuthorizationTokensOptionsKey { get; } =
        new(nameof(AuthorizationTokens));

    private readonly ILogger<PatreonAuthHeaderHandler> logger;
    private readonly AsyncRetryPolicy<HttpResponseMessage> policy;
    private readonly IPatreonTokenRefreshApi tokenRefreshApi;
    private readonly IOptions<PatreonClientConfig> patreonClientConfigOptions;

    private PatreonClientConfig PatreonClientConfig => patreonClientConfigOptions.Value;

    public PatreonAuthHeaderHandler(
        IOptions<PatreonClientConfig> patreonClientConfigOptions,
        IOptions<PatreonEndpointConfig> patreonEndpointConfigOptions,
        ILogger<PatreonAuthHeaderHandler> logger
    )
    {
        this.patreonClientConfigOptions = patreonClientConfigOptions;
        this.logger = logger;

        tokenRefreshApi = RestService.For<IPatreonTokenRefreshApi>(
            patreonEndpointConfigOptions.Value.BaseAddress?.ToString()
                ?? throw new InvalidOperationException("Base address is not set")
        );

        policy = Policy
            .HandleResult<HttpResponseMessage>(r =>
                // Refresh for Unauthorized or Forbidden
                r.StatusCode
                    is HttpStatusCode.Unauthorized
                        or HttpStatusCode.Forbidden
                // Check an existing Bearer token was sent
                && r.RequestMessage?.Headers.Authorization is { Scheme: "Bearer", Parameter: { } param }
                && !string.IsNullOrWhiteSpace(param)
                // Check and get our options key from the request
                && r.RequestMessage.Options.TryGetValue(AuthorizationTokensOptionsKey, out var authTokens)
                // Check that a refresh token exists
                && authTokens.RefreshToken is not null
            )
            .RetryAsync(OnRetryAsync);
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        return policy.ExecuteAsync(async () =>
        {
            // Add if Authorization object is set with at least AccessToken
            if (
                request.Options.TryGetValue(AuthorizationTokensOptionsKey, out var authTokens)
                && authTokens.AccessToken is not null
            )
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    authTokens.AccessToken
                );
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        });
    }

    private async Task OnRetryAsync(DelegateResult<HttpResponseMessage> result, int retryCount)
    {
        // Require authTokens with refresh token and Client ID + Secret
        if (
            result.Result.RequestMessage is not { } request
            || !request.Options.TryGetValue(AuthorizationTokensOptionsKey, out var authTokens)
            || string.IsNullOrEmpty(authTokens.RefreshToken)
            || string.IsNullOrEmpty(PatreonClientConfig.ClientId)
            || string.IsNullOrEmpty(PatreonClientConfig.ClientSecret)
        )
            return;

        logger.LogInformation("Refreshing access token for status ({StatusCode})", result.Result.StatusCode);

        var token = await tokenRefreshApi.GetToken(
            PatreonTokenRequest.FromRefreshToken(
                authTokens.RefreshToken,
                PatreonClientConfig.ClientId,
                PatreonClientConfig.ClientSecret
            )
        );

        // Update the new token
        authTokens.AccessToken = token.AccessToken;
        authTokens.AccessTokenExpiration = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
        authTokens.RefreshToken = token.RefreshToken;

        request.Options.Set(AuthorizationTokensOptionsKey, authTokens);

        logger.LogInformation("Access token refreshed");
    }
}
