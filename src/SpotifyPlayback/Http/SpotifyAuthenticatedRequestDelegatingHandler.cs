using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Pjfm.Application.Authentication;
using Pjfm.Common;
using Pjfm.Common.Http;
using SpotifyAPI.Web;

namespace SpotifyPlayback.Http
{
    public class SpotifyAuthenticatedRequestDelegatingHandler : DelegatingHandler
    {
        private readonly ISpotifyTokenService _spotifyTokenService;

        public SpotifyAuthenticatedRequestDelegatingHandler(ISpotifyTokenService spotifyTokenService)
        {
            _spotifyTokenService = spotifyTokenService;
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var typedRequest = Guard.IsType<HttpRequestMessage, DelegatingRequestMessage>(request);
            var userId = typedRequest.GetDelegatingParam("userId");
            
            Guard.NotNullOrEmpty(userId, nameof(userId));
            
            var accessTokenResult = await _spotifyTokenService.GetUserSpotifyAccessToken(userId);
            if (!accessTokenResult.IsSuccessful)
            {
                throw new APIException("Could not retrieve the user AccessToken");
            }
            
            typedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResult.AccessToken);
            
            var response = await base.SendAsync(typedRequest, cancellationToken);

            return response;
        }
    }
}