using System;
using System.Collections.Generic;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackScheduledTaskQueue
    {
        int Count { get; }
        void AddPlaybackScheduledNummer(PlaybackScheduledNummer playbackScheduledNummer);
        bool RemovePlaybackScheduledNummer(Guid groupId);
        IEnumerable<PlaybackScheduledNummer> GetDueNummers();
    }
}