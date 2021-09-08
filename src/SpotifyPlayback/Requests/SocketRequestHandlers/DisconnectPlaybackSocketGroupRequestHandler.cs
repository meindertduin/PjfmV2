using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.SocketRequestHandlers
{
    public class DisconnectPlaybackSocketGroupRequestHandler : IPlaybackSocketRequestHandler<DisconnectPlaybackGroupRequest>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public DisconnectPlaybackSocketGroupRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        public Task HandleAsync(DisconnectPlaybackGroupRequest request, SocketConnection socketConnection)
        {
            _playbackGroupCollection.RemoveUserFromGroup(new ListenerDto(socketConnection.ConnectionId, socketConnection.Principal));

            // TODO: pause spotify player for user once it's implemented
            
            return Task.CompletedTask;
        }
    }

    public class DisconnectPlaybackGroupRequest : IPlaybackRequest
    {
    }
}