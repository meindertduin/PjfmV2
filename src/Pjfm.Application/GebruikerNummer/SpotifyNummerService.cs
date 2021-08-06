using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var spotifyClient = new SpotifyClient(accessTokenResult.AccessToken);

            if (!accessTokenResult.IsSuccessful)
            {
                return;
            }

            var spotifyNummers = new List<SpotifyNummer>(150);

            foreach (var trackTermijn in Enum.GetValues<TrackTermijn>())
            {
                var nummers = await GetTermTracks(ref spotifyClient, trackTermijn);
                
                spotifyNummers.AddRange(nummers.Items?.Select(s => new SpotifyNummer()
                {
                    Titel = s.Name,
                    SpotifyNummerId = s.Id,
                    AangemaaktOp = DateTime.Now,
                    Artists = s.Artists.Select(a => a.Name),
                    TrackTermijn = trackTermijn,
                    NummerDuurMs = s.DurationMs,
                    GebruikerId = gebruikerId,
                }) ?? Array.Empty<SpotifyNummer>());   
            }

            if (spotifyNummers.Count > 0)
            {
                await _spotifyNummerRepository.SetGebruikerSpotifyNummers(spotifyNummers, gebruikerId);
            }
        }

        private Task<Paging<FullTrack>> GetTermTracks(ref SpotifyClient spotifyClient, TrackTermijn termijn)
        {
            return spotifyClient.Personalization.GetTopTracks(new PersonalizationTopRequest()
            {
                TimeRangeParam = ConvertTermijnToTimeRange(termijn),
                Limit = TermijnSpotifyNummersAmount,
            });
        }
        private PersonalizationTopRequest.TimeRange ConvertTermijnToTimeRange(TrackTermijn termijn)
        {
            switch (termijn)
            {
                case TrackTermijn.Kort:
                    return PersonalizationTopRequest.TimeRange.ShortTerm;
                case TrackTermijn.Middelmatig:
                    return PersonalizationTopRequest.TimeRange.MediumTerm;
                case TrackTermijn.Lang:
                    return PersonalizationTopRequest.TimeRange.LongTerm;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }

    public interface ISpotifyNummerService
    {
        Task UpdateGebruikerSpotifyNummers(string gebruikerId);
    }
}