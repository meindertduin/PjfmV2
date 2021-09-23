using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackQueue
    {
        Task<SpotifyTrackDto> GetNextSpotifyTrack();
        IEnumerable<SpotifyTrackDto> GetQueuedTracks(int amount);
        void ResetQueue();
        void SetTermijn(TrackTerm term);
    }
}