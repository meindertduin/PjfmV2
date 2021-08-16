using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroupGroupCollection : IPlaybackGroupCollection
    {
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<Guid, IPlaybackGroup> _playbackGroups = new();

        public event PlaybackGroupCreatedEvent playbackgroupCreatedEvent;

        public PlaybackGroupGroupCollection(IServiceProvider serviceProvider)
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

            playbackgroupCreatedEvent.Invoke(this, new PlaybackGroupCreatedEventArgs() { GroupId = groupId });
            return groupId;
        }

        public async Task<PlaybackScheduledNummer> GetGroupNewTrack(Guid groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            var groupNummer = await playbackGroup.GetNextNummer();
            return new PlaybackScheduledNummer()
            {
                SpotifyNummer = groupNummer,
                GroupId = groupId,
            };
        }

        public IEnumerable<string> GetGroupGebruikerIds(Guid groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            return playbackGroup.GetGroupListenerIds();
        }

        private IPlaybackGroup GetPlaybackGroup(Guid groupId)
        {
            if (_playbackGroups.TryGetValue(groupId, out var playbackGroup))
            {
                return playbackGroup;
            }
            
            throw new KeyNotFoundException("PlaybackGroup with GroupId could not be found.");
        }
    }

    public delegate void PlaybackGroupCreatedEvent(object sender, PlaybackGroupCreatedEventArgs args);

    public class PlaybackGroupCreatedEventArgs
    {
        public Guid GroupId { get; set; }
    }
}