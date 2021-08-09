using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain.SpotifyGebruikerData;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pjfm.Application.Common;

namespace Pjfm.Application.Spotify
{
    public interface ISpotifyAuthenticationService
    {
        Task<ServiceRequestResult<SpotifyAccessTokenRequestResult>> RequestAccessToken(string code);
        Task<ServiceRequestResult<SpotifyAccessTokenRefreshRequestResult>> RefreshAccessToken(string gebruikerId);
    }

    public class SpotifyAuthService : ISpotifyAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private static HttpClient _httpClient;
        private readonly ISpotifyGebruikersDataRepository _spotifyGebruikersDataRepository;

        public SpotifyAuthService(IConfiguration configuration, HttpClient httpClient,
            ISpotifyGebruikersDataRepository spotifyGebruikersDataRepository)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _spotifyGebruikersDataRepository = spotifyGebruikersDataRepository;
        }

        public async Task<ServiceRequestResult<SpotifyAccessTokenRequestResult>> RequestAccessToken(string code)
        {
            var redirectUrl = _configuration["Spotify:RedirectUrl"];
            var requestMessage = GetBaseTokenRequestMessage();

            requestMessage.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
            });

            var response = await _httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var resultContent = JsonConvert.DeserializeObject<SpotifyAccessTokenRequestResult>(
                    await response.Content.ReadAsStringAsync(), SpotifyApiHelpers.GetSpotifySerializerSettings());

                return ServiceRequestResult<SpotifyAccessTokenRequestResult>.Success(resultContent,
                    response.StatusCode);
            }

            return ServiceRequestResult<SpotifyAccessTokenRequestResult>.Fail(null, response.StatusCode);
        }

        public async Task<ServiceRequestResult<SpotifyAccessTokenRefreshRequestResult>> RefreshAccessToken(
            string gebruikerId)
        {
            using var client = _httpClientFactory.CreateClient();

            var requestMessage = GetBaseTokenRequestMessage();
            var gebruikerRefreshToken = await _spotifyGebruikersDataRepository.GetGebruikerRefreshToken(gebruikerId);

            requestMessage.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", gebruikerRefreshToken)
            });

            var response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var resultContent = JsonConvert.DeserializeObject<SpotifyAccessTokenRefreshRequestResult>(
                    await response.Content.ReadAsStringAsync(),
                    SpotifyApiHelpers.GetSpotifySerializerSettings());
                
                return ServiceRequestResult<SpotifyAccessTokenRefreshRequestResult>.Success(resultContent, response.StatusCode);
            }

            return ServiceRequestResult<SpotifyAccessTokenRefreshRequestResult>.Fail(null, response.StatusCode);
        }

        private HttpRequestMessage GetBaseTokenRequestMessage()
        {
            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];

            var clientCredentials = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");

            var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, new Uri(_configuration["Spotify:TokenEndpoint"]));

            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCredentials));

            return requestMessage;
        }
    }

    public abstract class AccessTokenRequestResult
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Score { get; set; }
    }

    public class SpotifyAccessTokenRequestResult : AccessTokenRequestResult
    {
        public string RefreshToken { get; set; }
    }

    public class SpotifyAccessTokenRefreshRequestResult : AccessTokenRequestResult
    {
    }
}