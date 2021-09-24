using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pjfm.Application.Authentication;
using Pjfm.Application.Common;
using Pjfm.Application.GebruikerNummer.Models;
using Pjfm.Common.Http;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyTrackClient : ISpotifyTrackClient
    {
        private readonly IConfiguration _configuration;
        private static HttpClient _client = null!;
        private const int AlbumImageSize = 300;

        public SpotifyTrackClient(ISpotifyTokenService spotifyTokenService, IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient(
                new SpotifyAuthenticatedRequestDelegatingHandler(spotifyTokenService, serviceProvider));
        }

        public async Task<SpotifyClientTrackstResult> GetSpotifyTracks(SpotifyTrackRequest spotifyTrackRequest, string userId)
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

            var result = await SendTracksRequest<SpotifyClientTrackstResult>(requestMessage);
            if (result == null)
            {
                return new SpotifyClientTrackstResult()
                {
                    Items = Enumerable.Empty<SpotifyClientTrackItemResult>(),
                    Total = 0,
                };
            }
            
            foreach (var spotifyTrackItemResult in result.Items)
            {
                spotifyTrackItemResult.TrackTerm = spotifyTrackRequest.TrackTerm;
            }

            return result;
        }

        public async Task<IEnumerable<SpotifyTrackDto>> SearchSpotifyTracks(string query, string userId)
        {
            var url = new StringBuilder(_configuration["Spotify:ApiBaseUrl"]);
            url.Append($"/search?query={query}");
            url.Append("&type=track");

            var requestMessage = new DelegatingRequestBuilder(HttpMethod.Get, url.ToString())
                .AddRequestParam(DelegatingRequestParams.UserId, userId)
                .Build();

            var result = await SendTracksRequest<SpotifyTrackSearchClientResult>(requestMessage);

            if (result == null)
            {
                return Enumerable.Empty<SpotifyTrackDto>();
            }

            return result.Tracks.Items.Select(x => x.GetTrackDto(AlbumImageSize));
        }

        private async Task<T?> SendTracksRequest<T>(HttpRequestMessage requestMessage)
        {
            var response = await _client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(),
                    SpotifyApiHelpers.GetSpotifySerializerSettings());
            }

            return default;
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
        Task<SpotifyClientTrackstResult> GetSpotifyTracks(SpotifyTrackRequest spotifyTrackRequest, string userId);
        Task<IEnumerable<SpotifyTrackDto>> SearchSpotifyTracks(string query, string userId);
    }
}