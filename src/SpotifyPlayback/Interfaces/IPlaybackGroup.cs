using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyNummer;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroup
    {
        Guid GroupId { get; }
        string GroupName { get; }
        Task<SpotifyNummer> GetNextNummer();
        IEnumerable<string> GetGroupListenerIds();
        bool AddLuisteraar(LuisteraarDto luisteraar);
        bool RemoveLuisteraar(LuisteraarDto luisteraar);
        bool ContainsLuisteraar(LuisteraarDto luisteraar);
        bool HasLuisteraars();
    }
}