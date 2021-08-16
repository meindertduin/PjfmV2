using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Services;

namespace SpotifyPlayback
{
    public class SpotifyPlaybackHostedService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private IPlaybackGroupCollection _playbackGroupCollection;

        public SpotifyPlaybackHostedService(IServiceProvider services)
        {
            _services = services;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();
            _playbackGroupCollection = scope.ServiceProvider.GetRequiredService<IPlaybackGroupCollection>();
            _playbackGroupCollection.playbackgroupCreatedEvent += AddNewGroupToScheduler;
            
            while (stoppingToken.IsCancellationRequested == false)
            {
                
            }
        }

        private void AddNewGroupToScheduler(object sender, PlaybackGroupCreatedEventArgs eventArgs)
        {
            Task.Run(async () =>
            {
                var newNummer = await _playbackGroupCollection.GetGroupNewTrack(eventArgs.GroupId);
                
            });
        }
    }
}