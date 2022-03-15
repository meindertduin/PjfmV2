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
        public IPlaybackGroup GetPlaybackGroup(string groupId);
        string CreateNewPlaybackGroup(SessionGroup sessionGroup);
        Task<PlaybackScheduledTrack> GetGroupNewTrack(string groupId);
        IEnumerable<ListenerDto> GetGroupListeners(string groupId);
        IEnumerable<Guid> GetGroupJoinedConnectionIds(string groupId);
        PlaybackGroupDto GetPlaybackGroupInfo(string groupId);
        IEnumerable<PlaybackGroupDto> GetPlaybackGroupsInfo();
        bool RemoveJoinedConnectionFromGroup(Guid connectionId, string groupId);
    }
}