using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SessionGroup;
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
        private ConcurrentDictionary<string, IPlaybackGroup> _playbackGroups = new();

        public event PlaybackGroupCreatedEvent PlaybackGroupCreatedEvent = null!;

        public PlaybackGroupCollection(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string CreateNewPlaybackGroup(SessionGroup sessionGroup)
        {
            var playbackQueue = GetNewPlaybackQueue();
            var groupId = sessionGroup.Id;

            playbackQueue.SetFillerQueueParticipantIds(sessionGroup.FillerQueueParticipants.Select(p => p.Id).ToArray());
            
            CreateAndAddPlaybackGroup(sessionGroup.GroupName, playbackQueue, groupId);
            
            PlaybackGroupCreatedEvent.Invoke(this, new PlaybackGroupCreatedEventArgs() { GroupId = groupId });
            
            return groupId;
        }

        private IPlaybackQueue GetNewPlaybackQueue()
        {
            using var scope = _serviceProvider.CreateScope();
            var playbackQueue = scope.ServiceProvider.GetRequiredService<IPlaybackQueue>();
            return playbackQueue;
        }
        
        private void CreateAndAddPlaybackGroup(string groupName, IPlaybackQueue playbackQueue, string groupId)
        {
            var playbackGroup = new PlaybackGroup(playbackQueue, groupId, groupName);
            _playbackGroups.TryAdd(groupId, playbackGroup);
        }

        public async Task<PlaybackScheduledTrack> GetGroupNewTrack(string groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            var groupTrack = await playbackGroup.GetNextTrack();
            
            return new PlaybackScheduledTrack()
            {
                SpotifyTrack = groupTrack,
                GroupId = groupId,
            };
        }
        
        public IEnumerable<ListenerDto> GetGroupListeners(string groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            return playbackGroup.GetGroupListeners();
        }

        public IEnumerable<Guid> GetGroupJoinedConnectionIds(string groupId)
        {
            var playbackGroup = GetPlaybackGroup(groupId);
            return playbackGroup.GetJoinedConnectionIds();
        }

        public PlaybackGroupDto GetPlaybackGroupInfo(string groupId)
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

        public bool RemoveJoinedConnectionFromGroup(Guid connectionId, string groupId)
        {
            if (_playbackGroups.TryGetValue(groupId, out var playbackGroup))
            {
                return playbackGroup.RemoveJoinedConnection(connectionId);
            }
            
            return false;
        }
        public IPlaybackGroup GetPlaybackGroup(string groupId)
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
        public string GroupId { get; set; }
    }
}