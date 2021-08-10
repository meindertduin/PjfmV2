using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pjfm.Common.Authentication;

namespace SpotifyPlayback.Models
{
    public class SocketConnection
    {
        private WebSocket _webSocket;
        private HttpContext _context;
        private IPjfmPrincipal _principal;

        public SocketConnection(WebSocket webSocket, HttpContext context)
        {
            _webSocket = webSocket;
            _context = context;
            _principal = context.User.GetPjfmPrincipal();
        }

        public async Task PollConnection(Action<WebSocketReceiveResult, byte[]> onMessageReceive)
        {
            var buffer = new byte[1024];

            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                onMessageReceive(result, buffer);
            }

            await CloseConnection();
        }

        public async Task CloseConnection()
        {
            if (_webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived or WebSocketState.CloseSent)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
            }
        }
    }
}