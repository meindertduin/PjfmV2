using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;
using SpotifyPlayback.Services;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroupCollection
    {
        event PlaybackGroupCreatedEvent PlaybackGroupCreatedEvent;
        Guid CreateNewPlaybackGroup(string groupName);
        Task<PlaybackScheduledTrack> GetGroupNewTrack(Guid groupId);
        IEnumerable<ListenerDto> GetGroupListeners(Guid groupId);
        IEnumerable<Guid> GetGroupJoinedConnectionIds(Guid groupId);
        PlaybackGroupDto GetPlaybackGroupInfo(Guid groupId);
        IEnumerable<PlaybackGroupDto> GetPlaybackGroupsInfo();
        bool JoinGroup(Guid groupId, Guid connectionId);
        bool RemoveJoinedConnectionFromGroup(Guid connectionId, Guid groupId);
        bool RemoveListenerFromGroup(Guid connectionId, Guid groupId);
        void ClearConnectionFromGroup(Guid connectionId, Guid groupId);
        bool ListenToGroup(Guid groupId, ListenerDto listener);
    }
}