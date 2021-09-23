using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Pjfm.Application.Authentication;
using Pjfm.Application.Spotify;
using Pjfm.Common;
using Pjfm.Common.Http;

namespace Pjfm.Application.Common
{
    public class SpotifyAuthenticatedRequestDelegatingHandler : DelegatingHandler
    {
        private readonly ISpotifyTokenService _spotifyTokenService;
        private readonly IServiceProvider _serviceProvider;

        public SpotifyAuthenticatedRequestDelegatingHandler(ISpotifyTokenService spotifyTokenService,
            IServiceProvider serviceProvider)
        {
            _spotifyTokenService = spotifyTokenService;
            _serviceProvider = serviceProvider;
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var typedRequest = Guard.IsType<HttpRequestMessage, DelegatingRequestMessage>(request);
            var userId = typedRequest.GetDelegatingParam(DelegatingRequestParams.UserId);

            Guard.NotNullOrEmpty(userId, nameof(userId));

            var accessTokenResult = await _spotifyTokenService.GetUserSpotifyAccessToken(userId);
            if (!accessTokenResult.IsSuccessful)
            {
                throw new NullReferenceException("AccessToken is null");
            }

            typedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResult.AccessToken);

            var response = await base.SendAsync(typedRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<SpotifyErrorResponse>(
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    SpotifyApiHelpers.GetSpotifySerializerSettings());

                if (errorResponse?.ErrorDescription == SpotifyErrorResponseDescriptions.TokenRevoked)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var spotifyAuthenticationRevoker =
                        scope.ServiceProvider.GetRequiredService<ISpotifyAuthenticationRevoker>();

                    await spotifyAuthenticationRevoker.RevokeUserSpotifyAuthentication(userId);
                }
            }

            return response;
        }
    }
}