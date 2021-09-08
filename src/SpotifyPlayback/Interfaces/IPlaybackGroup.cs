using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using SpotifyPlayback.Models.DataTransferObjects;
using SpotifyPlayback.Models.Socket;

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
        bool ContainsListeners(ListenerDto listener);
        bool ContainsJoinedConnectionId(Guid connectionId);
        bool HasListeners();
        PlaybackGroupDto GetPlaybackGroupInfo();
    }
}