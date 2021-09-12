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
            var spotifyPlaybackService = scope.ServiceProvider.GetRequiredService<ISpotifyPlaybackService>();
            _playbackGroupCollection.ClearConnectionFromGroup(socketConnection.ConnectionId, request.ConnectedGroupId);
            
            spotifyPlaybackService.PausePlaybackForUser(socketConnection.Principal.Id);
            
            return Task.CompletedTask;
        }
    }

    public class DisconnectPlaybackGroupRequest : IPlaybackRequest
    {
        public Guid ConnectedGroupId { get; set; }
    }
}