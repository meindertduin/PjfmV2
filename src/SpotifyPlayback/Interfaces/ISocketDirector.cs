using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface ISocketDirector
    {
        Task HandleSocketConnection(WebSocket socket, HttpContext context);
        Task BroadCastMessage<T>(SocketMessage<T> message);
        Task BroadCastMessageOverConnections<T>(SocketMessage<T> message, IEnumerable<Guid> connectionIds);
    }
}