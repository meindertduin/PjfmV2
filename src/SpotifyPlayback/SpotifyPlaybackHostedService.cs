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
        private IPlaybackGroupCollection _playbackGroupCollection = null!;
        private Timer? _playbackTimer;
        private IPlaybackScheduledTrackQueue _playbackScheduledTrackQueue = null!;
        private readonly IServiceProvider _services;

        public SpotifyPlaybackHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            
            _playbackGroupCollection = scope.ServiceProvider.GetRequiredService<IPlaybackGroupCollection>();
            _playbackScheduledTrackQueue = scope.ServiceProvider.GetRequiredService<IPlaybackScheduledTrackQueue>();
            
            _playbackGroupCollection.PlaybackGroupCreatedEvent += AddNewGroupToScheduler;
            
            // TODO: For now we create one at the start. In later versions more groups should be able to be created
            _playbackGroupCollection.CreateNewPlaybackGroup("Pjfm");

            _playbackTimer = new Timer(ExecuteAsync, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _playbackTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void ExecuteAsync(object? state)
        {
            Task.Run(async () =>
            {
                var dueTracks = _playbackScheduledTrackQueue.GetDueTracks();
                foreach (var dueTrack in dueTracks)
                {
                    await PlayScheduledTrack(dueTrack);
                }
            });
        }
        private void AddNewGroupToScheduler(object sender, PlaybackGroupCreatedEventArgs eventArgs)
        {
            Task.Run(async () =>
            {
                var nextTrack = await _playbackGroupCollection.GetGroupNewTrack(eventArgs.GroupId);
                await PlayScheduledTrack(nextTrack);
            });
        }

        private async Task PlayScheduledTrack(PlaybackScheduledTrack playbackScheduledTrack)
        {
            using var scope = _services.CreateScope();
            var spotifyPlaybackController = scope.ServiceProvider.GetRequiredService<ISpotifyPlaybackController>();
            
            var groupNewTrack = await _playbackGroupCollection.GetGroupNewTrack(playbackScheduledTrack.GroupId);
            groupNewTrack.DueTime = DateTime.Now + TimeSpan.FromMilliseconds(playbackScheduledTrack.SpotifyTrack.TrackDurationMs);
            
            _playbackScheduledTrackQueue.AddPlaybackScheduledTrack(groupNewTrack);
            await spotifyPlaybackController.PlaySpotifyTrackForUsers(playbackScheduledTrack);
        }

        public void Dispose()
        {
            _playbackTimer?.Dispose();
        }
    }
}