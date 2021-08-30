using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyNummer;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroup : IPlaybackGroup
    {
        private readonly IPlaybackQueue _playbackQueue;
        private SpotifyNummer? _currentlyPlayingNumber = null;
        private ConcurrentBag<string> _gebruikerIds = new();
        
        public Guid GroupId { get; private set; }
        public string GroupName { get; private set; } 

        public PlaybackGroup(IPlaybackQueue playbackQueue, Guid groupId, string groupName)
        {
            _playbackQueue = playbackQueue;
            GroupName = groupName;
            GroupId = groupId;
        }


        public async Task<SpotifyNummer> GetNextNummer()
        {
            var newNummer = await _playbackQueue.GetNextSpotifyNummer();
            _currentlyPlayingNumber = newNummer;

            return newNummer;
        }

        public IEnumerable<string> GetGroupListenerIds()
        {
            return _gebruikerIds.ToArray();
        }

        public bool AddLuisteraar(string gebruikerId)
        {
            if (!_gebruikerIds.Contains(gebruikerId))
            {
                _gebruikerIds.Add(gebruikerId);
                return true;
            }

            return false;
        }
        public bool HasLuisteraars()
        {
            return !_gebruikerIds.IsEmpty;
        }
    }
}