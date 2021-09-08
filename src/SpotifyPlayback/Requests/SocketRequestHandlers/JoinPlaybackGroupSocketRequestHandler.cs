using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Requests.SocketRequestHandlers
{
    public class JoinPlaybackGroupSocketRequestHandler : IPlaybackSocketRequestHandler<JoinPlaybackGroupSocketRequest>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public JoinPlaybackGroupSocketRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        public Task HandleAsync(JoinPlaybackGroupSocketRequest request, SocketConnection socketConnection)
        {
            var joinedGroup = _playbackGroupCollection.JoinGroup(request.GroupId,
                new ListenerDto(socketConnection.ConnectionId, socketConnection.Principal));

            if (!joinedGroup)
            {
                return Task.CompletedTask;
            }

            var playbackGroupInfo = _playbackGroupCollection.GetPlaybackGroupInfo(request.GroupId);
            var response = new SocketMessage<PlaybackUpdateMessageBody>()
            {
                MessageType = MessageType.PlaybackInfo,
                Body = new PlaybackUpdateMessageBody()
                {
                    GroupId = playbackGroupInfo.GroupId,
                    GroupName = playbackGroupInfo.GroupName,
                    CurrentlyPlayingTrack = playbackGroupInfo.CurrentlyPlayingTrack,
                }
            };
            return socketConnection.SendMessage(response.GetBytes());
        }
    }

    public class JoinPlaybackGroupSocketRequest : IPlaybackRequest
    {
        public Guid GroupId { get; set; }
    }
}