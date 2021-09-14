using System.Collections.Generic;
using System.Linq;
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
        private static HttpClient _httpClient = new();
        private static readonly string _spotifyApiBaseUrl = "https://api.spotify.com/v1";

        public async Task<bool> PlayTrackForUser(string accessToken, SpotifyPlayRequestDto content, string deviceId)
        {
            var url = "/me/player/play";
            if (!string.IsNullOrEmpty(deviceId))
            {
                url += $"?device_id={deviceId}";
            }
            
            var requestMessage = CreateBaseSpotifyRequestMessage(HttpMethod.Put, url, accessToken);
            
            var jsonString = JsonConvert.SerializeObject(content, SpotifyApiHelpers.GetSpotifySerializerSettings());
            requestMessage.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.SendAsync(requestMessage);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PausePlayer(string accessToken, string? deviceId = null)
        {
            var requestMessage = CreateBaseSpotifyRequestMessage(HttpMethod.Put, "/me/player/pause", accessToken);
            var response = await _httpClient.SendAsync(requestMessage);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<DeviceDto>> GetPlaybackDevices(string accessToken)
        {
            var requestMessage = CreateBaseSpotifyRequestMessage(HttpMethod.Get, "/me/player/devices", accessToken);
            var response = await _httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var devicesResult = JsonConvert.DeserializeObject<SpotifyPlaybackDevicesResponse>(await response.Content.ReadAsStringAsync(), SpotifyApiHelpers.GetSpotifySerializerSettings());
                if (devicesResult != null)
                {
                    return devicesResult.Devices;
                }
            }
            
            return Enumerable.Empty<DeviceDto>();
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

    public class SpotifyPlaybackDevicesResponse
    {
        public IEnumerable<DeviceDto> Devices { get; set; } = null!;
    }
}