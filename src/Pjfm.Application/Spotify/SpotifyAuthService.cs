using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Pjfm.Application.Spotify
{
    public class SpotifyAuthService : ISpotifyAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SpotifyAuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<SpotifyAccessTokenResult> RequestAccessToken(string code)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
            var redirectUrl = _configuration["Spotify:RedirectUrl"];
            
            var clientCredentials = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
            
            var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, new Uri(_configuration["Spotify:TokenEndpoint"]));

            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCredentials));
            
            requestMessage.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
            });

            var response = await httpClient.SendAsync(requestMessage);

            return new SpotifyAccessTokenResult()
            {
                SpotifyAccessToken = "test",
                SpotifyRefreshToken = "test"
            };
        }
    }
    
    public interface ISpotifyAuthenticationService
    {
        Task<SpotifyAccessTokenResult> RequestAccessToken(string code);
    }
}