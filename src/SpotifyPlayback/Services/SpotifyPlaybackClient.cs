using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pjfm.Application.Authentication;
using Pjfm.Application.Common;
using Pjfm.Common.Http;
using SpotifyPlayback.Http;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class SpotifyPlaybackClient : ISpotifyPlaybackClient
    {
        private static HttpClient _httpClient = null!;
        private static readonly string _spotifyApiBaseUrl = "https://api.spotify.com/v1";

        public SpotifyPlaybackClient(ISpotifyTokenService userTokenService)
        {
            _httpClient = new HttpClient(new SpotifyAuthenticatedRequestDelegatingHandler(userTokenService));
        }
        
        public async Task<bool> PlayTrackForUser(string userId, SpotifyPlayRequestDto content, string deviceId)
        {
            var url = "/me/player/play";
            if (!string.IsNullOrEmpty(deviceId))
            {
                url += $"?device_id={deviceId}";
            }
            
            var requestMessage = CreateBaseSpotifyRequestMessage(HttpMethod.Put, url, userId);

            var jsonString = JsonConvert.SerializeObject(content, SpotifyApiHelpers.GetSpotifySerializerSettings());
            requestMessage.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.SendAsync(requestMessage);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PausePlayer(string userId, string? deviceId = null)
        {
            var requestMessage = CreateBaseSpotifyRequestMessage(HttpMethod.Put, "/me/player/pause", userId);
            var response = await _httpClient.SendAsync(requestMessage);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<DeviceDto>> GetPlaybackDevices(string userId)
        {
            var requestMessage = CreateBaseSpotifyRequestMessage(HttpMethod.Get, "/me/player/devices", userId);
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
        
        private DelegatingRequestMessage CreateBaseSpotifyRequestMessage(HttpMethod method, string url, string userId)
        {
            var request = new DelegatingRequestMessage(method, _spotifyApiBaseUrl + url);
            request.SetDelegatingParam("userId", userId);
            return request;
        }
    }

    public class SpotifyPlaybackDevicesResponse
    {
        public IEnumerable<DeviceDto> Devices { get; set; } = null!;
    }
}