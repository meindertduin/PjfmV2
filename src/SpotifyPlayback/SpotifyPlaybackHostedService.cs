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
        private IPlaybackGroepCollection _playbackGroepCollection = null!;
        private Timer? _timer;
        private IPlaybackScheduledTaskQueue _playbackScheduledTaskQueue = null!;

        public SpotifyPlaybackHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _playbackGroepCollection = _services.GetRequiredService<IPlaybackGroepCollection>();
            _playbackScheduledTaskQueue = _services.GetRequiredService<IPlaybackScheduledTaskQueue>();
            
            _playbackGroepCollection.PlaybackGroupCreatedEvent += AddNewGroepToScheduler;
            
            // TODO: For now we create one at the start. In later versions more groups should be able to be created
            _playbackGroepCollection.CreateNewPlaybackGroup("Pjfm");

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
            Task.Run(async () =>
            {
                var dueNummers = _playbackScheduledTaskQueue.GetDueNummers();
                foreach (var dueNummer in dueNummers)
                {
                    await PlayScheduledNummer(dueNummer);
                }
            });
        }

        private void AddNewGroepToScheduler(object sender, PlaybackGroupCreatedEventArgs eventArgs)
        {
            Task.Run(async () =>
            {
                var newNummer = await _playbackGroepCollection.GetGroupNewTrack(eventArgs.GroupId);
                var nextNewNummer = await _playbackGroepCollection.GetGroupNewTrack(eventArgs.GroupId);
                
                nextNewNummer.DueTime = DateTime.Now + TimeSpan.FromMilliseconds(nextNewNummer.SpotifyTrack.TrackDurationMs);
                
                await PlayScheduledNummer(newNummer);
            });
        }

        private async Task PlayScheduledNummer(PlaybackScheduledNummer playbackScheduledNummer)
        {
            var groupNewTrack = await _playbackGroepCollection.GetGroupNewTrack(playbackScheduledNummer.GroupId);
            groupNewTrack.DueTime = DateTime.Now + TimeSpan.FromMilliseconds(groupNewTrack.SpotifyTrack.TrackDurationMs);
            _playbackScheduledTaskQueue.AddPlaybackScheduledNummer(groupNewTrack);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}