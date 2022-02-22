using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Requests.SocketRequestHandlers;

namespace SpotifyPlayback
{
    /// <summary>
    /// This hosted service watches the connection state of all the Socket connections, and handles connection states
    /// that might be undefined. In a perfect world, this class does not exist, but sometimes there are fallthrough cases
    /// where the Connection is not connected anymore, but the user is still listening to a playback group.
    ///
    /// If you have a better idea, please let me know
    /// </summary>
    public class ConnectionWatcherHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _watchTimer = null!;
        public ConnectionWatcherHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _watchTimer = new Timer(ExecuteAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private void ExecuteAsync(object? state)
        {
            Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();

                var socketConnectionCollection = scope.ServiceProvider.GetRequiredService<ISocketConnectionCollection>();
                var socketConnections = socketConnectionCollection.GetSocketConnections().ToArray();

                if (socketConnections.All(c => c.IsConnected))
                {
                    return;
                }

                var playbackRequestDispatcher = scope.ServiceProvider.GetRequiredService<IPlaybackRequestDispatcher>();

                foreach (var connection in socketConnections)
                {
                    var socketConnection = (SocketConnection) connection;
                    if (!socketConnection.IsConnected)
                    {
                        if (socketConnection.Principal.IsAuthenticated())
                        {
                            await playbackRequestDispatcher.HandlePlaybackSocketRequest(
                                new DisconnectPlaybackGroupRequest(), socketConnection);
                        }

                        socketConnectionCollection.RemoveUserFromConnectionIdMap(socketConnection);
                        socketConnectionCollection.RemoveSocket(socketConnection.ConnectionId);
                    }
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _watchTimer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _watchTimer.Dispose();
        }
    }
}