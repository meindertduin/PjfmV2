using System.Threading.Tasks;
using Domain.SpotifyTrack;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackQueue
    {
        Task<SpotifyTrack> GetNextSpotifyNummer();
        void ResetQueue();
        void SetTermijn(TrackTerm term);
    }
}