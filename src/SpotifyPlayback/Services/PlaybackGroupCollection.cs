using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroupCollection : IPlaybackGroupCollection
    {
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<Guid, IPlaybackGroup> _playbackGroups = new();

        public event PlaybackGroupCreatedEvent playbackgroupCreatedEvent = null!;

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

        public IEnumerable<PlaybackGroupDto> GetPlaybackGroupsInfo()
        {
            var groupsData = new List<PlaybackGroupDto>();
            
            foreach (var playbackGroup in _playbackGroups.Values)
            {
                groupsData.Add(playbackGroup.GetPlaybackGroupInfo());
            }

            return groupsData;
        }

        public bool JoinGroup(Guid groupId, LuisteraarDto luisteraar)
        {
            var retrievedGroup = _playbackGroups.TryGetValue(groupId, out var playbackGroup);
            if (retrievedGroup)
            {
                return playbackGroup!.AddLuisteraar(luisteraar);
            }

            return false;
        }

        public bool RemoveGebruikerFromGroup(LuisteraarDto luisteraar)
        {
            // TODO: This is highly inefficient on larger scale, but will work for now In the future we might be needing
            // to think of saving the groupId where the user is connected with somewhere
            foreach (var playbackGroup in _playbackGroups.Values)
            {
                if (playbackGroup.ContainsLuisteraar(luisteraar))
                {
                    return playbackGroup.RemoveLuisteraar(luisteraar);
                }
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