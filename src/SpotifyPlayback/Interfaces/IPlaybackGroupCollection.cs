using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SessionGroup;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;
using SpotifyPlayback.Services;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroupCollection
    {
        event PlaybackGroupCreatedEvent PlaybackGroupCreatedEvent;
        public IPlaybackGroup GetPlaybackGroup(Guid groupId);
        Guid CreateNewPlaybackGroup(SessionGroup sessionGroup);
        Task<PlaybackScheduledTrack> GetGroupNewTrack(Guid groupId);
        IEnumerable<ListenerDto> GetGroupListeners(Guid groupId);
        IEnumerable<Guid> GetGroupJoinedConnectionIds(Guid groupId);
        PlaybackGroupDto GetPlaybackGroupInfo(Guid groupId);
        IEnumerable<PlaybackGroupDto> GetPlaybackGroupsInfo();
        bool RemoveJoinedConnectionFromGroup(Guid connectionId, Guid groupId);
    }
}