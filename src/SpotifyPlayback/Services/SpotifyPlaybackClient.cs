using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pjfm.Application.Common;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class SpotifyPlaybackClient : ISpotifyPlaybackClient
    {
        private static HttpClient _httpClient = null!;
        private static readonly string _spotifyApiBaseUrl = "https://api.spotify.com/v1";

        public SpotifyPlaybackClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PlayTrackForUser(string accessToken, SpotifyPlayRequestDto content, string? deviceId = null)
        {
            var url = "/me/player/play";
            if (!string.IsNullOrEmpty(deviceId))
            {
                url += $"?device_id={deviceId}";
            }
            
            var requestMessage = CreateBaseSpotifyRequestMessage(HttpMethod.Put, url, accessToken);
            var response = await _httpClient.SendAsync(requestMessage);

            var jsonString = JsonConvert.SerializeObject(content, SpotifyApiHelpers.GetSpotifySerializerSettings());
            requestMessage.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PausePlayer(string accessToken, string? deviceId = null)
        {
            var requestMessage = CreateBaseSpotifyRequestMessage(HttpMethod.Put, "/me/player/pause", accessToken);
            var response = await _httpClient.SendAsync(requestMessage);

            return response.IsSuccessStatusCode;
        }
        
        private HttpRequestMessage CreateBaseSpotifyRequestMessage(HttpMethod method, string url, string? accessToken)
        {
            var requestMessage = new HttpRequestMessage(method, _spotifyApiBaseUrl + url);
            if (accessToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return requestMessage;
        }
    }
}