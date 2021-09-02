using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroep : IPlaybackGroep
    {
        private readonly IPlaybackQueue _playbackQueue;
        private SpotifyTrack? _currentlyPlayingTrack = null;
        private SpotifyTrack? _nextTrack = null;

        private List<ListenerDto> _luisteraars = new();
        private readonly object luisteraarsLock = new();

        public Guid GroupId { get; private set; }
        public string GroupName { get; private set; }

        public PlaybackGroep(IPlaybackQueue playbackQueue, Guid groupId, string groupName)
        {
            _playbackQueue = playbackQueue;
            GroupName = groupName;
            GroupId = groupId;
        }

        public async Task<SpotifyTrack> GetNextTrack()
        {
            var newNummer = await _playbackQueue.GetNextSpotifyNummer();
            
            if (_currentlyPlayingTrack == null)
            {
                _currentlyPlayingTrack = newNummer;
            }
            else if (_nextTrack == null)
            {
                _nextTrack = newNummer;
            }
            else
            {
                _currentlyPlayingTrack = _nextTrack;
                _nextTrack = newNummer;
            }

            return newNummer;
        }
        public IEnumerable<string> GetGroupListenerIds()
        {
            return _luisteraars.Select(x => x.UserId);
        }

        public bool AddListener(ListenerDto listener)
        {
            if (!_luisteraars.Contains(listener))
            {
                lock (luisteraarsLock)
                {
                    _luisteraars.Add(listener);
                }

                return true;
            }

            return false;
        }

        public bool RemoveListener(ListenerDto listener)
        {
            lock (luisteraarsLock)
            {
                return _luisteraars.Remove(listener);
            }
        }

        public bool ContainsListeners(ListenerDto listener)
        {
            return _luisteraars.Contains(listener);
        }

        public bool HasListeners()
        {
            return !_luisteraars.Any();
        }

        public PlaybackGroupDto GetPlaybackGroupInfo()
        {
            return new()
            {
                GroupId = GroupId,
                GroupName = GroupName,
                ListenersCount = _luisteraars.Count,
                CurrentlyPlayingNummer = _currentlyPlayingTrack,
            };
        }
    }
}