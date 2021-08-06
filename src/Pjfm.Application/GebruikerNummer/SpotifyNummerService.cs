using System;
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

        private const int MaxSpotifyNummersPerUser = 150;

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
            
            var spotifyClient = new SpotifyClient(accessTokenResult.AccessToken);

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
                        Titel = x.Name,
                        SpotifyNummerId = x.Id,
                        AangemaaktOp = DateTime.Now,
                        Artists = x.Artists.Select(s => s.Name),
                        TrackTermijn = TrackTermijn.Kort,
                        NummerDuurMs = x.DurationMs,
                        GebruikerId = gebruikerId,
                    }) ?? Enumerable.Empty<SpotifyNummer>(), gebruikerId);
            }
        }
    }

    public interface ISpotifyNummerService
    {
        Task UpdateGebruikerSpotifyNummers(string gebruikerId);
    }
}