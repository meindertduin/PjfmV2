using System;
using System.Collections.Generic;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackScheduledTaskQueue
    {
        int Count { get; }
        void AddPlaybackScheduledTrack(PlaybackScheduledTracks playbackScheduledTrack);
        bool RemovePlaybackScheduledTrack(Guid groupId);
        IEnumerable<PlaybackScheduledTracks> GetDueTracks();
    }
}