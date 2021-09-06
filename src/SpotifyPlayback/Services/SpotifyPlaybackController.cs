using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Services
{
    public class SpotifyPlaybackController : ISpotifyPlaybackController
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly IServiceProvider _serviceProvider;

        public SpotifyPlaybackController(IPlaybackGroupCollection playbackGroupCollection, IServiceProvider serviceProvider)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _serviceProvider = serviceProvider;
        }
        
        public Task PlaySpotifyTrackForUsers(PlaybackScheduledTrack playbackScheduledTrack)
        {
            var spotifyPlaybackService = _serviceProvider.GetRequiredService<ISpotifyPlaybackService>();
            var listeners = _playbackGroupCollection.GetGroupListeners(playbackScheduledTrack.GroupId);
            return spotifyPlaybackService.PlayNextTrackForUsers(listeners.ToArray(), playbackScheduledTrack.SpotifyTrack.SpotifyTrackId);
        }

        public Task PauseSpotifyPlayerUser()
        {
            throw new System.NotImplementedException();
        }
    }
}