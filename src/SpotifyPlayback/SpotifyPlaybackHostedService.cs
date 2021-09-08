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
        private ISpotifyPlaybackController _spotifyPlaybackController = null!;
        private IPlaybackGroupCollection _playbackGroupCollection = null!;
        private Timer? _timer;
        private IPlaybackScheduledTrackQueue _playbackScheduledTrackQueue = null!;

        public SpotifyPlaybackHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _playbackGroupCollection = _services.GetRequiredService<IPlaybackGroupCollection>();
            _playbackScheduledTrackQueue = _services.GetRequiredService<IPlaybackScheduledTrackQueue>();
            _spotifyPlaybackController = _services.GetRequiredService<ISpotifyPlaybackController>();
            
            _playbackGroupCollection.PlaybackGroupCreatedEvent += AddNewGroupToScheduler;
            
            // TODO: For now we create one at the start. In later versions more groups should be able to be created
            _playbackGroupCollection.CreateNewPlaybackGroup("Pjfm");

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
                var newTrack = await _playbackGroupCollection.GetGroupNewTrack(eventArgs.GroupId);
                var nextNewTrack = await _playbackGroupCollection.GetGroupNewTrack(eventArgs.GroupId);
                
                nextNewTrack.DueTime = DateTime.Now + TimeSpan.FromMilliseconds(nextNewTrack.SpotifyTrack.TrackDurationMs);
                
                await PlayScheduledTrack(newTrack);
            });
        }

        private async Task PlayScheduledTrack(PlaybackScheduledTrack playbackScheduledTrack)
        {
            var groupNewTrack = await _playbackGroupCollection.GetGroupNewTrack(playbackScheduledTrack.GroupId);
            groupNewTrack.DueTime = DateTime.Now + TimeSpan.FromMilliseconds(groupNewTrack.SpotifyTrack.TrackDurationMs);
            _playbackScheduledTrackQueue.AddPlaybackScheduledTrack(groupNewTrack);
            await _spotifyPlaybackController.PlaySpotifyTrackForUsers(playbackScheduledTrack);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}