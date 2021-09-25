using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Requests.SocketRequestHandlers
{
    public class JoinPlaybackGroupSocketRequestHandler : IPlaybackSocketRequestHandler<JoinPlaybackGroupSocketRequest>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly ISocketDirector _socketDirector;

        public JoinPlaybackGroupSocketRequestHandler(IPlaybackGroupCollection playbackGroupCollection, ISocketDirector socketDirector)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _socketDirector = socketDirector;
        }
        public Task HandleAsync(JoinPlaybackGroupSocketRequest request, SocketConnection socketConnection)
        {
            var socketConnectedGroupId = socketConnection.GetConnectedPlaybackGroupId();
            if (socketConnectedGroupId != null)
            {
                if (socketConnectedGroupId == request.GroupId)
                {
                    return Task.CompletedTask;
                }
                
                _playbackGroupCollection.RemoveJoinedConnectionFromGroup(socketConnection.ConnectionId, socketConnectedGroupId.Value);
            }

            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(request.GroupId);
            var joinedGroup = playbackGroup.AddJoinedConnectionId(socketConnection.ConnectionId);
            
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
                    QueuedTracks = playbackGroupInfo.QueuedTracks,
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