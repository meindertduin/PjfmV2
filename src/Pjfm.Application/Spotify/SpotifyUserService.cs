using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pjfm.Application.Common;
using Pjfm.Application.Spotify.models;

namespace Pjfm.Application.Spotify
{
    public interface ISpotifyUserService
    {
        Task<SpotifyClientUserResult?> GetSpotifyUserData(string accessToken);
    }

    public class SpotifyUserService : ISpotifyUserService 
    {
        private readonly IConfiguration _configuration;
        private static HttpClient _client = new HttpClient();

        public SpotifyUserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SpotifyClientUserResult?> GetSpotifyUserData(string accessToken)
        {
            var url = $"{_configuration["Spotify:ApiBaseUrl"]}/me";
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new ("Bearer", accessToken);

            var result = await _client.SendAsync(requestMessage);

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<SpotifyClientUserResult>(await result.Content.ReadAsStringAsync(),
                    SpotifyApiHelpers.GetSpotifySerializerSettings());
            }

            return null;
        }
    }
}