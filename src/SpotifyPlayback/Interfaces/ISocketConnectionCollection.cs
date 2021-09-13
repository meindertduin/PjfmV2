using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface ISocketConnectionCollection
    {
        void RemoveUserFromConnectionIdMap(SocketConnection socketConnection);
        bool RemoveSocket(Guid connectionId);
        IEnumerable<ISocketConnection> GetSocketConnections();
        bool TryGetUserSocketConnection(string userId, [MaybeNullWhen(false)] out ISocketConnection socketConnection);
        bool SetSocketConnectedGroupId(Guid connectionId, Guid groupId);
        bool ClearSocketConnectedGroupId(Guid connectionId);
    }
}