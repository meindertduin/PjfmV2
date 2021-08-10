using System.Collections.Generic;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface ISocketDirector
    {
        bool AddSocket(WebSocket socket, HttpContext context, string gebruikerId);
        bool RemoveSocket(string gebruikerId);
        IEnumerable<SocketConnection> GetSocketConnections();
    }
}