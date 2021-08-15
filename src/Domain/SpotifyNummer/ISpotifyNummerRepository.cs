using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.SpotifyNummer
{
    public interface ISpotifyNummerRepository
    {
        Task<List<SpotifyNummer>> GetGebruikerSpotifyNummersByGebruikersId(string gebruikersId);
        Task SetGebruikerSpotifyNummers(IEnumerable<SpotifyNummer> spotifyNummers, string gebruikerId);

        Task<List<SpotifyNummer>> GetRandomGebruikersSpotifyNummers(IEnumerable<string> gebruikerIds,
            IEnumerable<TrackTermijn> termijnen, int amount);
    }
}