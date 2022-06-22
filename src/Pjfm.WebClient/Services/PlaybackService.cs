using Newtonsoft.Json;
using SpotifyPlayback.Models.DataTransferObjects;

namespace Pjfm.WebClient.Services;

public class PlaybackService
{
    private static HttpClient _client = new ();

    public PlaybackService()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5004/api/playback/groups");
    }

    public async Task<List<PlaybackGroupDto>> GetPlaybackGroups()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5004/api/playback/groups");

        var response = await _client.SendAsync(message);

        if (response.IsSuccessStatusCode)
        {
            var resultContent = JsonConvert.DeserializeObject<List<PlaybackGroupDto>>(
                await response.Content.ReadAsStringAsync());

            return resultContent;
        }

        return new List<PlaybackGroupDto>();
    }
    
    
}