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

            var groupId = socketConnection.GetListeningPlaybackGroupId();
            if (string.IsNullOrEmpty(groupId))
            {
                return Task.CompletedTask;
            }
            
            var spotifyPlaybackService = scope.ServiceProvider.GetRequiredService<ISpotifyPlaybackService>();

            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(groupId);
            var userWasListener = playbackGroup.RemoveListener(socketConnection.ConnectionId);
            
            _playbackGroupCollection.RemoveJoinedConnectionFromGroup(socketConnection.ConnectionId, groupId);

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