using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Pjfm.Common;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroupCollection : IPlaybackGroupCollection
    {
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<Guid, IPlaybackGroup> _playbackGroups = new();

        public event PlaybackGroupCreatedEvent PlaybackGroupCreatedEvent = null!;

        public PlaybackGroupCollection(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Guid CreateNewPlaybackGroup(string groupName)
        {
            using var scope = _serviceProvider.CreateScope();
            var playbackQueue = scope.ServiceProvider.GetRequiredService<IPlaybackQueue>();
            var groupId = Guid.NewGuid();

            var playbackGroup = new PlaybackGroup(playbackQueue, groupId, groupName);

            _playbackGroups.TryAdd(groupId, playbackGroup);

            PlaybackGroupCreatedEvent.Invoke(this, new PlaybackGroupCreatedEventArgs() { GroupId = groupId });
            return groupId;
        }

        public async Task<PlaybackScheduledTrack> GetGroupNewTrack(Guid groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            var groupTrack = await playbackGroup.GetNextTrack();
            
            return new PlaybackScheduledTrack()
            {
                SpotifyTrack = groupTrack,
                GroupId = groupId,
            };
        }

        public IEnumerable<ListenerDto> GetGroupListeners(Guid groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            return playbackGroup.GetGroupListeners();
        }

        public IEnumerable<Guid> GetGroupJoinedConnectionIds(Guid groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            return playbackGroup.GetJoinedConnectionIds();
        }

        public PlaybackGroupDto GetPlaybackGroupInfo(Guid groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            Guard.NotNull(playbackGroup, nameof(playbackGroup));

            return playbackGroup.GetPlaybackGroupInfo();
        }

        public IEnumerable<PlaybackGroupDto> GetPlaybackGroupsInfo()
        {
            var groupsData = new List<PlaybackGroupDto>();
            
            foreach (var playbackGroup in _playbackGroups.Values)
            {
                groupsData.Add(playbackGroup.GetPlaybackGroupInfo());
            }

            return groupsData;
        }

        public bool JoinGroup(Guid groupId, Guid connectionId)
        {
            var retrievedGroup = _playbackGroups.TryGetValue(groupId, out var playbackGroup);
            if (retrievedGroup)
            {
                return playbackGroup!.AddJoinedConnectionId(connectionId);
            }

            return false;
        }

        public bool RemoveJoinedConnectionFromGroup(Guid connectionId, Guid groupId)
        {
            if (_playbackGroups.TryGetValue(groupId, out var playbackGroup))
            {
                return playbackGroup.RemoveJoinedConnection(connectionId);
            }
            
            return false;
        }

        public bool ListenToGroup(Guid groupId, ListenerDto listener)
        {
            var retrievedGroup = _playbackGroups.TryGetValue(groupId, out var playbackGroup);
            if (retrievedGroup)
            {
                return playbackGroup!.AddListener(listener);
            }

            return false;
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