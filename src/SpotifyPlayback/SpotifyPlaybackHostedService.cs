using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback
{
    public class SpotifyPlaybackHostedService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public SpotifyPlaybackHostedService(IServiceProvider services)
        {
            _services = services;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();
            var socketDirector = scope.ServiceProvider.GetRequiredService<ISocketDirector>();
            while (stoppingToken.IsCancellationRequested == false)
            {
                await Task.Delay(1000, stoppingToken);
                var sockets = socketDirector.GetSocketConnections();

                foreach (var socket in sockets)
                {
                    var message = new PlaybackSocketMessage<string>()
                    {
                        Body = "Ok",
                        ContentType = PlaybackMessageContentType.PlaybackUpdate,
                        MessageType = MessageType.Playback,
                    };

                    await socket.SendMessage(message.GetBytes());
                }
            }
        }
    }
}