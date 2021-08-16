using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroupCollection
    {
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<Guid, IPlaybackGroup> _playbackGroups = new();

        public PlaybackGroupCollection(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Guid CreateNewPlaybackGroup()
        {
            using var scope = _serviceProvider.CreateScope();
            var playbackQueue = scope.ServiceProvider.GetRequiredService<IPlaybackQueue>();

            var playbackGroup = new PlaybackGroup(playbackQueue);
            var groupId = Guid.NewGuid();

            _playbackGroups.TryAdd(groupId, playbackGroup);

            return groupId;
        }

        public async Task<PlaybackScheduledNummer> GetGroupNewTrack(Guid groupId)
        {
            if (_playbackGroups.TryGetValue(groupId, out var playbackGroup))
            {
                var groupNummer = await playbackGroup.GetNextNummer();
                return new PlaybackScheduledNummer()
                {
                    SpotifyNummer = groupNummer,
                    GroupId = groupId,
                };
            }
            
            throw new KeyNotFoundException("PlaybackGroup with GroupId could not be found.");
        }
    }
}