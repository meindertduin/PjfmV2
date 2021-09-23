using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pjfm.Application.Authentication;
using Pjfm.Application.Common;
using Pjfm.Common.Http;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyTrackClient : ISpotifyTrackClient
    {
        private readonly IConfiguration _configuration;
        private static HttpClient _client = null!;

        public SpotifyTrackClient(ISpotifyTokenService spotifyTokenService, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient(
                new SpotifyAuthenticatedRequestDelegatingHandler(spotifyTokenService, serviceProvider));
        }
        
        public async Task<SpotifyTracksResult> GetSpotifyTracks(SpotifyTrackRequest spotifyTrackRequest, string userId)
        {
            var url = new StringBuilder(_configuration["Spotify:ApiBaseUrl"]);
            
            url.Append($"/me/top/tracks?time_range={ConvertTrackTermToTimeRangeString(spotifyTrackRequest.TrackTerm)}");
            url.Append($"&limit={spotifyTrackRequest.PageSize}");
            
            if (spotifyTrackRequest.Page > 1)
            {
                url.Append($"&offset={spotifyTrackRequest.Offset}");
            }

            var requestMessage = new DelegatingRequestBuilder(HttpMethod.Get, url.ToString())
                .AddRequestParam(DelegatingRequestParams.UserId, userId)
                .Build();

            var response = await _client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<SpotifyTracksResult>(await response.Content.ReadAsStringAsync(),
                    SpotifyApiHelpers.GetSpotifySerializerSettings());

                if (result != null)
                {
                    foreach (var spotifyTrackItemResult in result.Items)
                    {
                        spotifyTrackItemResult.TrackTerm = spotifyTrackRequest.TrackTerm;
                    }
                }

                return result;
            }

            return new SpotifyTracksResult()
            {
                Items = Enumerable.Empty<SpotifyTrackItemResult>(),
                Total = 0,
            };
        }

        private static string ConvertTrackTermToTimeRangeString(TrackTerm term) =>
            term switch
            {
                TrackTerm.Short => "short_term",
                TrackTerm.Medium => "medium_term",
                TrackTerm.Long => "long_term",
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    public class SpotifyTrackRequest : PagedRequest
    {
        public TrackTerm TrackTerm { get; set; }
    }

    public interface ISpotifyTrackClient
    {
        Task<SpotifyTracksResult> GetSpotifyTracks(SpotifyTrackRequest spotifyTrackRequest, string userId);
    }
}