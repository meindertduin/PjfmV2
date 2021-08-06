using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyNummer;
using Pjfm.Application.Authentication;
using SpotifyAPI.Web;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyNummerService : ISpotifyNummerService
    {
        private readonly ISpotifyNummerRepository _spotifyNummerRepository;
        private readonly ISpotifyTokenService _spotifyTokenService;

        private const int TermijnSpotifyNummersAmount = 50;

        public SpotifyNummerService(ISpotifyNummerRepository spotifyNummerRepository, ISpotifyTokenService spotifyTokenService)
        {
            _spotifyNummerRepository = spotifyNummerRepository;
            _spotifyTokenService = spotifyTokenService;
        }

        public async Task UpdateGebruikerSpotifyNummers(string gebruikerId)
        {
            var accessTokenResult = await _spotifyTokenService.GetGebruikerSpotifyAccessToken(gebruikerId);

            if (!accessTokenResult.IsSuccessful)
            {
                return;
            }

            var spotifyTracks = await GetAllTermsSpotifyTracks(accessTokenResult.AccessToken);

            var spotifyNummers = new List<SpotifyNummer>(150);
            foreach (var spotifyTrackPageResult in spotifyTracks)
            {
                spotifyNummers.AddRange(spotifyTrackPageResult.Items?.Select(s => new SpotifyNummer()
                {
                    Titel = s.Name,
                    SpotifyNummerId = s.Id,
                    AangemaaktOp = DateTime.Now,
                    Artists = s.Artists.Select(a => a.Name),
                    TrackTermijn = TrackTermijn.Kort,
                    NummerDuurMs = s.DurationMs,
                    GebruikerId = gebruikerId,
                }) ?? Array.Empty<SpotifyNummer>());   
            }

            if (spotifyNummers.Count > 0)
            {
                await _spotifyNummerRepository.SetGebruikerSpotifyNummers(spotifyNummers, gebruikerId);
            }
        }

        private async Task<Paging<FullTrack>[]> GetAllTermsSpotifyTracks(string accessToken)
        {
            var spotifyClient = new SpotifyClient(accessToken);

            var retrieveTrackTasks = new Task<Paging<FullTrack>>[3];
            var enumValues = Enum.GetValues<PersonalizationTopRequest.TimeRange>();
            for (int i = 0; i < enumValues.Length; i++)
            {
                var retrieveTracksTask = spotifyClient.Personalization.GetTopTracks(new PersonalizationTopRequest()
                {
                    TimeRangeParam = enumValues[i],
                    Limit = TermijnSpotifyNummersAmount,
                });
                retrieveTrackTasks[i] = retrieveTracksTask;
            }

            return await Task.WhenAll(retrieveTrackTasks);
        }
    }

    public interface ISpotifyNummerService
    {
        Task UpdateGebruikerSpotifyNummers(string gebruikerId);
    }
}