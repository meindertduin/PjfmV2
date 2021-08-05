using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyNummer;
using SpotifyAPI.Web;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyNummerService : ISpotifyNummerService
    {
        private readonly ISpotifyNummerRepository _spotifyNummerRepository;

        private const int MaxSpotifyNummersPerUser = 150;

        public SpotifyNummerService(ISpotifyNummerRepository spotifyNummerRepository)
        {
            _spotifyNummerRepository = spotifyNummerRepository;
        }

        public async Task UpdateGebruikerSpotifyNummers(string gebruikerId, string spotifyAccessToken)
        {
            var spotifyClient = new SpotifyClient(spotifyAccessToken);

            var shortTermTracks = await spotifyClient.Personalization.GetTopTracks(new PersonalizationTopRequest()
            {
                TimeRangeParam = PersonalizationTopRequest.TimeRange.ShortTerm,
                Limit = 50,
            });
            var gebruikerNummers = await _spotifyNummerRepository.GetGebruikerSpotifyNummersByGebruikersId(gebruikerId);

            if (gebruikerNummers.Count != MaxSpotifyNummersPerUser)
            {
                await _spotifyNummerRepository.SetGebruikerSpotifyNummers(shortTermTracks.Items?.Select(x =>
                    new SpotifyNummer()
                    {
                        GebruikerId = gebruikerId,
                        AangemaaktOp = DateTime.Now,
                        Artists = x.Artists.Select(s => s.Name),
                        TrackTermijn = TrackTermijn.Kort,
                        NummerDuurMs = x.DurationMs,
                        SpotifyNummerId = x.Id,
                        Titel = x.Name,
                    }) ?? Enumerable.Empty<SpotifyNummer>(), gebruikerId);
            }
        }
    }

    public interface ISpotifyNummerService
    {
    }
}