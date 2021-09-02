using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroep
    {
        Guid GroupId { get; }
        string GroupName { get; }
        Task<SpotifyTrack> GetNextTrack();
        IEnumerable<string> GetGroupListenerIds();
        bool AddListener(ListenerDto listener);
        bool RemoveListener(ListenerDto listener);
        bool ContainsListeners(ListenerDto listener);
        bool HasListeners();
        PlaybackGroupDto GetPlaybackGroupInfo();
    }
}