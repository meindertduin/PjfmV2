using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pjfm.Application.Common;

namespace Pjfm.Application.Spotify
{
    public interface ISpotifyAuthenticationService
    {
        Task<ServiceRequestResult<SpotifyAccessTokenRequestResult>> RequestAccessToken(string code);
    }

    public class SpotifyAuthService : ISpotifyAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SpotifyAuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceRequestResult<SpotifyAccessTokenRequestResult>> RequestAccessToken(string code)
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
            if (response.IsSuccessStatusCode)
            {
                var resultContent = JsonConvert.DeserializeObject<SpotifyAccessTokenRequestResult>(
                    await response.Content.ReadAsStringAsync(), new JsonSerializerSettings()
                    {
                        ContractResolver = new CustomContractResolver()
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    });

                return ServiceRequestResult<SpotifyAccessTokenRequestResult>.Success(resultContent, response.StatusCode);
            }
            
            return ServiceRequestResult<SpotifyAccessTokenRequestResult>.Fail(null, response.StatusCode);
        }
    }

    public class SpotifyAccessTokenRequestResult
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string Score { get; set; }
    }
}