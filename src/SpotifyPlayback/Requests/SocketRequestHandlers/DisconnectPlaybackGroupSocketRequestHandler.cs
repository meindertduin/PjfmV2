using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Requests.SocketRequestHandlers
{
    public class DisconnectPlaybackGroupSocketRequestHandler : IPlaybackSocketRequestHandler<DisconnectPlaybackGroupRequest>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly IServiceProvider _serviceProvider;

        public DisconnectPlaybackGroupSocketRequestHandler(IPlaybackGroupCollection playbackGroupCollection, IServiceProvider serviceProvider)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _serviceProvider = serviceProvider;
        }
        public Task HandleAsync(DisconnectPlaybackGroupRequest request, SocketConnection socketConnection)
        {
            using var scope = _serviceProvider.CreateScope();

            var groupId = socketConnection.GetConnectedPlaybackGroupId();
            if (!groupId.HasValue)
            {
                return Task.CompletedTask;
            }
            
            var spotifyPlaybackService = scope.ServiceProvider.GetRequiredService<ISpotifyPlaybackService>();
            var userWasListener = _playbackGroupCollection.RemoveListenerFromGroup(socketConnection.ConnectionId, groupId.Value);
            _playbackGroupCollection.RemoveJoinedConnectionFromGroup(socketConnection.ConnectionId, groupId.Value);

            if (userWasListener)
            {
                spotifyPlaybackService.PausePlaybackForUser(socketConnection.Principal.Id);
            }
            
            return Task.CompletedTask;
        }
    }

    public class DisconnectPlaybackGroupRequest : IPlaybackRequest
    {
    }
}