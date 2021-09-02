using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;
using SpotifyPlayback.Services;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroepCollection
    {
        event PlaybackGroupCreatedEvent PlaybackGroupCreatedEvent;
        Guid CreateNewPlaybackGroup(string groupName);
        Task<PlaybackScheduledNummer> GetGroupNewTrack(Guid groupId);
        IEnumerable<string> GetGroupUserIds(Guid groupId);
        IEnumerable<PlaybackGroupDto> GetPlaybackGroupsInfo();
        bool JoinGroup(Guid groupId, ListenerDto listener);
        bool RemoveUserFromGroup(ListenerDto listener);
    }
}