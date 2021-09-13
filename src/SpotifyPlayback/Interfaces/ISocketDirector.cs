using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface ISocketDirector
    {
        Task HandleSocketConnection(WebSocket socket, HttpContext context);
        bool RemoveSocket(Guid connectionId);
        IEnumerable<ISocketConnection> GetSocketConnections();
        bool TryGetUserSocketConnection(string userId, [MaybeNullWhen(false)] out ISocketConnection socketConnection);
        Task BroadCastMessage<T>(SocketMessage<T> message);
        Task BroadCastMessageOverConnections<T>(SocketMessage<T> message, IEnumerable<Guid> connectionIds);
        bool SetSocketConnectedGroupId(Guid connectionId, Guid groupId);
        bool ClearSocketConnectedGroupId(Guid connectionId);
    }
}