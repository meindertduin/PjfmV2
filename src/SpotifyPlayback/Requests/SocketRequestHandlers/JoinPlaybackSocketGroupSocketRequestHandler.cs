using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.SocketRequestHandlers
{
    public class JoinPlaybackSocketGroupSocketRequestHandler : IPlaybackSocketRequestHandler<JoinPlaybackGroupRequest>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public JoinPlaybackSocketGroupSocketRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        public Task HandleAsync(JoinPlaybackGroupRequest request, SocketConnection socketConnection)
        {
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

            return socketConnection.SendMessage(responseModel.GetBytes());
        }
    }

    public abstract class JoinPlaybackGroupRequest : IPlaybackRequest
    {
        public Guid GroupId { get; set; }
    }

    public class JoinPlaybackGroupSocketResponse
    {
        public bool SuccessfullyJoined { get; set; }
    }
}