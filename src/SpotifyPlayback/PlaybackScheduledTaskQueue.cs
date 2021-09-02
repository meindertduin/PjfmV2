using System;
using System.Collections.Generic;
using System.Linq;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback
{
    public class PlaybackScheduledTaskQueue : IPlaybackScheduledTaskQueue
    {
        private const int DefaultCapacity = 5;

        private object _queueLock = new ();
        private List<PlaybackScheduledNummer> _playbackScheduledNummers = new();
        public int Count => _playbackScheduledNummers.Count;

        public void AddPlaybackScheduledNummer(PlaybackScheduledNummer playbackScheduledNummer)
        {
            var hasInserted = false;
            lock (_queueLock)
            {
                for (int i = 0; i < _playbackScheduledNummers.Count; i++)
                {
                    if (_playbackScheduledNummers[i].DueTime > playbackScheduledNummer.DueTime)
                    {
                        _playbackScheduledNummers.Insert(i, playbackScheduledNummer);
                        hasInserted = true;
                        break;
                    }
                }

                if (!hasInserted)
                {
                    _playbackScheduledNummers.Add(playbackScheduledNummer);
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

        public IEnumerable<PlaybackScheduledNummer> GetDueNummers()
        {
            var time = DateTime.Now;
            var dueScheduledPlaybackNummers = new List<PlaybackScheduledNummer>();

            lock (_queueLock)
            {
                foreach (var playbackScheduledNummer in _playbackScheduledNummers)
                {
                    if (playbackScheduledNummer.DueTime <= time)
                    {
                        _playbackScheduledNummers.Remove(playbackScheduledNummer);
                        dueScheduledPlaybackNummers.Add(playbackScheduledNummer);
                    }
                }
            }

            return dueScheduledPlaybackNummers;
        }
    }
}