using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback
{
    public class PlaybackSocketDirector : ISocketDirector
    {
        private static readonly ConcurrentDictionary<string, SocketConnection> Connections = new();
        
        public async Task HandleSocketConnection(WebSocket socket, HttpContext context)
        {
            var socketConnection = new SocketConnection(socket, context);
            if (Connections.TryAdd(Guid.NewGuid().ToString(), socketConnection))
            {
                await socketConnection.PollConnection((result, buffer) =>
                {
                    
                });
            }
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