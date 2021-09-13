using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Services
{
    public class SocketConnectionCollection : ISocketConnectionCollection
    {
        private static readonly ConcurrentDictionary<Guid, ISocketConnection> Connections = new();
        private static readonly ConcurrentDictionary<string, Guid> UserConnectionIdMap = new();
        
        public void RemoveUserFromConnectionIdMap(SocketConnection socketConnection)
        {
            if (socketConnection.Principal.IsAuthenticated())
            {
                UserConnectionIdMap.Remove(socketConnection.Principal.Id, out _);
            }
        }

        public bool RemoveSocket(Guid connectionId)
        {
            return Connections.Remove(connectionId, out _);
        }

        public IEnumerable<ISocketConnection> GetSocketConnections()
        {
            return Connections.Values;
        }

        public bool TryGetUserSocketConnection(string userId, [MaybeNullWhen(false)] out ISocketConnection socketConnection)
        {
            var hasFoundConnectionId = UserConnectionIdMap.TryGetValue(userId, out var connectionId);
            if (!hasFoundConnectionId)
            {
                socketConnection = null;
                return false;
            }
            var hasFoundSocketConnection = Connections.TryGetValue(connectionId, out socketConnection);
            return hasFoundSocketConnection;
        }
        
        public bool SetSocketConnectedGroupId(Guid connectionId, Guid groupId)
        {
            if (Connections.TryGetValue(connectionId, out var socketConnection))
            {
                socketConnection.SetConnectedPlaybackGroupId(groupId);
                return true;
            }
            
            return false;
        }

        public bool ClearSocketConnectedGroupId(Guid connectionId)
        {
            if (Connections.TryGetValue(connectionId, out var socketConnection))
            {
                socketConnection.ClearConnectedPlaybackGroupId();
                return true;
            }

            return false;
        }
    }
}