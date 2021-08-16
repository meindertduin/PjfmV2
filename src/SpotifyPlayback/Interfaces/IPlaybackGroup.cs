using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyNummer;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroup
    {
        Task<SpotifyNummer> GetNextNummer();
        IEnumerable<string> GetGroupListenerIds();
    }
}