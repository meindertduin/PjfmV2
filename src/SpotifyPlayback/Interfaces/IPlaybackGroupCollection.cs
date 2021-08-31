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
        event PlaybackGroupCreatedEvent playbackgroupCreatedEvent;
        Guid CreateNewPlaybackGroup(string groupName);
        Task<PlaybackScheduledNummer> GetGroupNewTrack(Guid groupId);
        IEnumerable<string> GetGroupGebruikerIds(Guid groupId);
        IEnumerable<PlaybackGroupDto> GetPlaybackGroupsInfo();
        bool JoinGroup(Guid groupId, LuisteraarDto luisteraar);
        bool RemoveGebruikerFromGroup(LuisteraarDto luisteraar);
    }
}