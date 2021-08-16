using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Services;

namespace SpotifyPlayback
{
    public class SpotifyPlaybackHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private IPlaybackGroupCollection _playbackGroupCollection = null!;
        private Timer? _timer;
        private IPlaybackScheduledTaskQueue _playbackScheduledTaskQueue = null!;

        public SpotifyPlaybackHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            _playbackGroupCollection = scope.ServiceProvider.GetRequiredService<IPlaybackGroupCollection>();
            _playbackScheduledTaskQueue = scope.ServiceProvider.GetRequiredService<IPlaybackScheduledTaskQueue>();
            
            _playbackGroupCollection.playbackgroupCreatedEvent += AddNewGroupToScheduler;

            _timer = new Timer(ExecuteAsync, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void ExecuteAsync(object? state)
        {
            Task.Run(() =>
            {
                var dueNummers = _playbackScheduledTaskQueue.GetDueNummers();
                foreach (var dueNummer in dueNummers)
                {
                    PlayScheduledNummer(dueNummer);
                }
            });
        }

        private void AddNewGroupToScheduler(object sender, PlaybackGroupCreatedEventArgs eventArgs)
        {
            Task.Run(async () =>
            {
                var newNummer = await _playbackGroupCollection.GetGroupNewTrack(eventArgs.GroupId);
                var nextNewNummer = await _playbackGroupCollection.GetGroupNewTrack(eventArgs.GroupId);
                
                PlayScheduledNummer(newNummer);
                _playbackScheduledTaskQueue.AddPlaybackScheduledNummer(nextNewNummer);
            });
        }

        private void PlayScheduledNummer(PlaybackScheduledNummer playbackScheduledNummer)
        {
            
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}