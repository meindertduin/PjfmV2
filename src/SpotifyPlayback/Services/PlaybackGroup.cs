using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyNummer;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroup : IPlaybackGroup
    {
        private readonly IPlaybackQueue _playbackQueue;
        private SpotifyNummer? _currentlyPlayingNumber = null;
        private List<LuisteraarDto> _luisteraars = new();

        private readonly object luisteraarsLock = new();

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
            return _luisteraars.Select(x => x.GebruikerId);
        }

        public bool AddLuisteraar(LuisteraarDto luisteraar)
        {
            if (!_luisteraars.Contains(luisteraar))
            {
                lock (luisteraarsLock)
                {
                    _luisteraars.Add(luisteraar);
                }
                return true;
            }

            return false;
        }

        public bool RemoveLuisteraar(LuisteraarDto luisteraar)
        {
            lock (luisteraarsLock)
            {
                return _luisteraars.Remove(luisteraar);
            }
        }

        public bool ContainsLuisteraar(LuisteraarDto luisteraar)
        {
            return _luisteraars.Contains(luisteraar);
        }

        public bool HasLuisteraars()
        {
            return !_luisteraars.Any();
        }
    }
}