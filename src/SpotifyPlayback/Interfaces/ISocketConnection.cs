using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Pjfm.Common.Authentication;

namespace SpotifyPlayback.Interfaces
{
    public interface ISocketConnection
    {
        Guid ConnectionId { get; init; }
        public bool IsConnected { get; }
        IPjfmPrincipal Principal { get; init; }
        Task PollConnection(Action<WebSocketReceiveResult, byte[]> onMessageReceive);
        Task SendMessage(byte[] message);
        Guid? GetConnectedPlaybackGroupId();
        void SetConnectedPlaybackGroupId(Guid groupId);
        void ClearConnectedPlaybackGroupId();
    }
}