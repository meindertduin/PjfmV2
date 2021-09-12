using System.Threading.Tasks;
using Domain.SpotifyTrack;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackQueue
    {
        Task<SpotifyTrackDto> GetNextSpotifyTrack();
        void ResetQueue();
        void SetTermijn(TrackTerm term);
    }
}