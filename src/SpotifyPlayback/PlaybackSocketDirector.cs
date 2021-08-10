using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback
{
    public class PlaybackSocketDirector : ISocketDirector
    {
        private static readonly ConcurrentDictionary<string, SocketConnection> Connections = new();
        
        public bool AddSocket(WebSocket socket, HttpContext context, string gebruikerId)
        {
            var socketConnection = new SocketConnection(socket, context);
            return Connections.TryAdd(gebruikerId, socketConnection);
        }

        public bool RemoveSocket(string gebruikerId)
        {
            return Connections.Remove(gebruikerId, out _);
        }

        public IEnumerable<SocketConnection> GetSocketConnections()
        {
            return Connections.Values;
        }
    }
}