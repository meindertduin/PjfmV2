using System;
using System.Collections.Generic;
using System.Linq;
using SpotifyPlayback.Models;

namespace SpotifyPlayback
{
    public class PlaybackScheduledTaskQueue
    {
        private const int DefaultCapacity = 5;

        private object _queueLock = new ();
        private List<PlaybackScheduledNummer> _playbackScheduledNummers = new();
        public int Count => _playbackScheduledNummers.Count;

        public void AddPlaybackScheduledNummer(PlaybackScheduledNummer playbackScheduledNummer)
        {
            lock (_queueLock)
            {
                for (int i = 0; i < _playbackScheduledNummers.Count; i++)
                {
                    if (_playbackScheduledNummers[i].DueTime > playbackScheduledNummer.DueTime)
                    {
                        _playbackScheduledNummers.Insert(i, playbackScheduledNummer);
                    }
                }
            }
        }

        public bool RemovePlaybackScheduledNummer(Guid groupId)
        {
            lock (_queueLock)
            {
                var scheduledNummer = _playbackScheduledNummers.FirstOrDefault(p => p.GroupId == groupId);
                if (scheduledNummer != null)
                {
                    _playbackScheduledNummers.Remove(scheduledNummer);
                    return true;
                }
            }

            return false;
        }

        public void NextTick()
        {
            var time = DateTime.Now;

            lock (_queueLock)
            {
                foreach (var playbackScheduledNummer in _playbackScheduledNummers)
                {
                    if (playbackScheduledNummer.DueTime <= time)
                    {
                        _playbackScheduledNummers.Remove(playbackScheduledNummer);
                        
                        // TODO: handle the scheduledNummers
                    }
                }
            }
        }
    }
}