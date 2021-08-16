using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Models;
using SpotifyPlayback.Services;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroupCollection
    {
        event PlaybackGroupCreatedEvent playbackgroupCreatedEvent;
        Guid CreateNewPlaybackGroup();
        Task<PlaybackScheduledNummer> GetGroupNewTrack(Guid groupId);
        IEnumerable<string> GetGroupGebruikerIds(Guid groupId);
    }
}