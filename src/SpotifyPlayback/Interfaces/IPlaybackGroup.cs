using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Pjfm.Application.GebruikerNummer.Models;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Interfaces
{
    public interface IPlaybackGroup
    {
        Guid GroupId { get; }
        string GroupName { get; }
        Task<SpotifyTrackDto> GetNextTrack();
        IEnumerable<ListenerDto> GetGroupListeners();
        IEnumerable<Guid> GetJoinedConnectionIds();
        bool AddListener(ListenerDto listener);
        bool AddJoinedConnectionId(Guid connectionId);
        bool RemoveJoinedConnection(Guid connectionId);
        bool RemoveListener(Guid connectionId);
        bool ContainsListeners(ListenerDto listener);
        bool ContainsJoinedConnectionId(Guid connectionId);
        bool HasListeners();
        PlaybackGroupDto GetPlaybackGroupInfo();
        void AddTracksToQueue(IEnumerable<SpotifyTrackDto> tracks);
    }
}