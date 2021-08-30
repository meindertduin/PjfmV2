using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyNummer;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroup
    {
        Guid GroupId { get; }
        string GroupName { get; }
        Task<SpotifyNummer> GetNextNummer();
        IEnumerable<string> GetGroupListenerIds();
        bool AddLuisteraar(string gebruikerId);
        bool HasLuisteraars();
    }
}