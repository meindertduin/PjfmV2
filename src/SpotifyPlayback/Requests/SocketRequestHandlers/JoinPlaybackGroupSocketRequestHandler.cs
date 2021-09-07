using System;
using System.Threading.Tasks;
using SpotifyPlayback.Authentication;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.SocketRequestHandlers
{
    public class JoinPlaybackGroupSocketRequestHandler : IPlaybackSocketRequestHandler<JoinPlaybackGroupRequest>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public JoinPlaybackGroupSocketRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        public Task HandleAsync(JoinPlaybackGroupRequest request, SocketConnection socketConnection)
        {
            AuthenticationHelper.HandleUnauthorized(socketConnection, out var isAuthorized);
            if (!isAuthorized)
            {
                return Task.CompletedTask;
            }
            
            var listener = new ListenerDto(socketConnection.Principal.Id, socketConnection.ConnectionId);
            var hasJoined = _playbackGroupCollection.JoinGroup(request.GroupId, listener);
            
            var responseModel = new SocketMessage<JoinPlaybackGroupSocketResponse>()
            {
                MessageType = MessageType.JoinedGroupStatusUpdate,
                Body = new JoinPlaybackGroupSocketResponse()
                {
                    SuccessfullyJoined = hasJoined,
                }
            };

            if (hasJoined)
            {
                responseModel.Body.PlaybackGroupInfo = _playbackGroupCollection.GetPlaybackGroupInfo(request.GroupId);
            }

            return socketConnection.SendMessage(responseModel.GetBytes());
        }
    }

    public class JoinPlaybackGroupRequest : IPlaybackRequest
    {
        public Guid GroupId { get; set; }
    }

    public class JoinPlaybackGroupSocketResponse
    {
        public bool SuccessfullyJoined { get; set; }
        public PlaybackGroupDto PlaybackGroupInfo { get; set; }
    }
}