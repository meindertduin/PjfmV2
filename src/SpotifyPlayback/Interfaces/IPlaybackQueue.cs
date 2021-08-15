using System.Threading.Tasks;
using Domain.SpotifyNummer;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackQueue
    {
        Task<SpotifyNummer> GetNextSpotifyNummer();
        void ResetQueue();
        void SetTermijn(TrackTermijn termijn);
    }
}