using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroup
    {
        Guid GroupId { get; }
        string GroupName { get; }
        Task<SpotifyTrack> GetNextTrack();
        IEnumerable<ListenerDto> GetGroupListeners();
        bool AddListener(ListenerDto listener);
        bool RemoveListener(ListenerDto listener);
        bool ContainsListeners(ListenerDto listener);
        bool HasListeners();
        PlaybackGroupDto GetPlaybackGroupInfo();
    }
}