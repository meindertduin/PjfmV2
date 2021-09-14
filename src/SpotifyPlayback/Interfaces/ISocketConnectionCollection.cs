using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface ISocketConnectionCollection
    {
        public bool TryAddUserToConnectionIdMap(string userId, Guid connectionId);
        void RemoveUserFromConnectionIdMap(SocketConnection socketConnection);
        bool TryAddSocket(Guid connectionId, SocketConnection socketConnection);
        bool RemoveSocket(Guid connectionId);
        IEnumerable<ISocketConnection> GetSocketConnections();
        IEnumerable<ISocketConnection> GetSocketConnections(IEnumerable<Guid> connectionIds);
        bool TryGetUserSocketConnection(string userId, [MaybeNullWhen(false)] out ISocketConnection socketConnection);
        bool SetSocketConnectedGroupId(Guid connectionId, Guid groupId);
        bool ClearSocketConnectedGroupId(Guid connectionId);
    }
}