using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Requests.SocketRequestHandlers;

namespace SpotifyPlayback
{
    public class PlaybackSocketDirector : ISocketDirector
    {
        private readonly IPlaybackRequestDispatcher _playbackRequestDispatcher;
        private readonly ISocketRequestHandler _socketRequestHandler;
        private readonly ISocketConnectionCollection _socketConnectionCollection;
        public PlaybackSocketDirector(IPlaybackRequestDispatcher playbackRequestDispatcher, ISocketRequestHandler socketRequestHandler, ISocketConnectionCollection socketConnectionCollection)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
            _socketRequestHandler = socketRequestHandler;
            _socketConnectionCollection = socketConnectionCollection;
        }
        
        public async Task HandleSocketConnection(WebSocket socket, HttpContext context)
        {
            var socketConnection = new SocketConnection(socket, context, Guid.NewGuid());
            if (_socketConnectionCollection.TryAddSocket(socketConnection.ConnectionId, socketConnection))
            {
                if (socketConnection.Principal.IsAuthenticated())
                {
                    _socketConnectionCollection.TryAddUserToConnectionIdMap(socketConnection.Principal.Id, socketConnection.ConnectionId);
                }
                
                var response = new PlaybackSocketMessage<int>()
                {
                    MessageType = MessageType.ConnectionEstablished,
                };

                await socketConnection.SendMessage(response.GetBytes());
                
                await socketConnection.PollConnection((result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        _socketRequestHandler.HandleSocketRequest(buffer, socketConnection);
                    }
                });

                // If the connection isn't polling anymore it means that the connection is likely closed
                await HandleConnectionClose(socketConnection);
            }
        }

        private async Task HandleConnectionClose(SocketConnection socketConnection)
        {
            if (!socketConnection.IsConnected)
            {
                if (socketConnection.Principal.IsAuthenticated())
                {
                    await _playbackRequestDispatcher.HandlePlaybackSocketRequest(new DisconnectPlaybackGroupRequest(), socketConnection);
                }

                _socketConnectionCollection.RemoveUserFromConnectionIdMap(socketConnection);
                _socketConnectionCollection.RemoveSocket(socketConnection.ConnectionId);
            }
        }


        public async Task BroadCastMessage<T>(SocketMessage<T> message)
        {
            var socketConnections = _socketConnectionCollection.GetSocketConnections();
            await SendMessageToConnections(message, socketConnections);
        }

        public async Task BroadCastMessageOverConnections<T>(SocketMessage<T> message, IEnumerable<Guid> connectionIds)
        {
            var socketConnections = _socketConnectionCollection.GetSocketConnections(connectionIds);
            
            await SendMessageToConnections(message, socketConnections);
        }

        private async Task SendMessageToConnections<T>(SocketMessage<T> message,
            IEnumerable<ISocketConnection> socketConnections)
        {
            var messageBytes = message.GetBytes();
            var sendMessageTasks = new List<Task>();
            
            foreach (var connection in socketConnections)
            {
                sendMessageTasks.Add(connection.SendMessage(messageBytes));
            }

            await Task.WhenAll(sendMessageTasks);
        }
    }
}