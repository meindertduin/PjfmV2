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
        bool RemoveSocket(string gebruikerId);
        IEnumerable<SocketConnection> GetSocketConnections();
    }
}