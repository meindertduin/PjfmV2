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
        private Guid? _connectedPlaybackGroup;
        public Guid ConnectionId { get; init; }
        public IPjfmPrincipal Principal { get; init; }
        public bool IsConnected => _webSocket.State is WebSocketState.Open;

        public SocketConnection(WebSocket webSocket, HttpContext context, Guid connectionId)
        {
            _webSocket = webSocket;
            _context = context;
            ConnectionId = connectionId;
            Principal = context.User.GetPjfmPrincipal();
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

        private async Task CloseConnection()
        {
            if (_webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived or WebSocketState.CloseSent)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
            }
        }

        public Task SendMessage(byte[] message)
        {
            if (IsConnected)
            {
                return _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true,
                    CancellationToken.None);
            }
            
            return Task.CompletedTask;
        }

        public Guid? GetConnectedPlaybackGroupId()
        {
            return _connectedPlaybackGroup;
        }

        public void SetConnectedPlaybackGroupId(Guid groupId)
        {
            _connectedPlaybackGroup = groupId;
        }

        public void ClearConnectedPlaybackGroupId()
        {
            _connectedPlaybackGroup = null;
        }
    }
}