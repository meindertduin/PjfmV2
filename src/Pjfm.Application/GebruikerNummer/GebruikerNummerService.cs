using System;
using System.Threading.Tasks;
using Pjfm.Application.Authentication;
using SpotifyAPI.Web;

namespace Pjfm.Application.GebruikerNummer
{
    public class GebruikerNummerService : IGebruikerNummerService
    {
        private readonly IGebruikerTokenService _gebruikerTokenService;

        public GebruikerNummerService(IGebruikerTokenService gebruikerTokenService)
        {
            _gebruikerTokenService = gebruikerTokenService;
        }
        
        public async Task UpdateGebruikerNummers(string gebruikerId, string spotifyAccessToken)
        {
            var spotifyClient = new SpotifyClient(spotifyAccessToken);

            var shortTermTracks = await spotifyClient.Personalization.GetTopTracks(new PersonalizationTopRequest()
            {
                TimeRangeParam = PersonalizationTopRequest.TimeRange.ShortTerm,
                Limit = 50,
            });
            
        }
    }

    public interface IGebruikerNummerService
    {
    }
}