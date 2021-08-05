using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.SpotifyNummer
{
    public interface ISpotifyNummerRepository
    {
        Task<List<SpotifyNummer>> GetGebruikerSpotifyNummersByGebruikersId(string gebruikersId);
        Task SetGebruikerSpotifyNummers(IEnumerable<SpotifyNummer> spotifyNummers, string gebruikerId);
    }
}