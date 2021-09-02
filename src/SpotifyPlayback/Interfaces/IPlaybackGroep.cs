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
        bool AddListener(LuisteraarDto luisteraar);
        bool RemoveListener(LuisteraarDto luisteraar);
        bool ContainsListeners(LuisteraarDto luisteraar);
        bool HasListeners();
        PlaybackGroupDto GetPlaybackGroupInfo();
    }
}