using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Pjfm.Application.GebruikerNummer.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackQueue
    {
        Task<SpotifyTrackDto?> GetNextSpotifyTrack();
        IEnumerable<SpotifyTrackDto> GetQueuedTracks(int amount);
        void ResetQueue();
        void SetTermijn(TrackTerm term);
        SpotifyTrackDto? AddTracksToQueue(IEnumerable<SpotifyTrackDto> tracks, SpotifyTrackDto? scheduledNextTrack);
    }
}