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
        public void SetFillerQueueParticipantIds(string[] ids);
        void ResetQueue();
        void SetTermijn(TrackTerm term);
        SpotifyTrackDto? AddRequestsToQueue(IEnumerable<SpotifyTrackDto> tracks, SpotifyTrackDto? scheduledTrack,
            string userId);
    }
}