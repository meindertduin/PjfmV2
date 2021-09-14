using System;
using System.Collections.Generic;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackScheduledTrackQueue
    {
        int Count { get; }
        void AddPlaybackScheduledTrack(PlaybackScheduledTrack playbackScheduledTrack);
        IEnumerable<PlaybackScheduledTrack> GetScheduledTracks();
        bool RemovePlaybackScheduledTrack(Guid groupId);
        IEnumerable<PlaybackScheduledTrack> GetDueTracks();
    }
}