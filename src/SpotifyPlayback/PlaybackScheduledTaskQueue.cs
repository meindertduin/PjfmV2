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
        private List<PlaybackScheduledTracks> _playbackScheduledTracks = new();
        public int Count => _playbackScheduledTracks.Count;

        public void AddPlaybackScheduledTrack(PlaybackScheduledTracks playbackScheduledTrack)
        {
            var hasInserted = false;
            lock (_queueLock)
            {
                for (int i = 0; i < _playbackScheduledTracks.Count; i++)
                {
                    if (_playbackScheduledTracks[i].DueTime > playbackScheduledTrack.DueTime)
                    {
                        _playbackScheduledTracks.Insert(i, playbackScheduledTrack);
                        hasInserted = true;
                        break;
                    }
                }

                if (!hasInserted)
                {
                    _playbackScheduledTracks.Add(playbackScheduledTrack);
                }
            }
        }

        public bool RemovePlaybackScheduledTrack(Guid groupId)
        {
            lock (_queueLock)
            {
                var scheduledTrack = _playbackScheduledTracks.FirstOrDefault(p => p.GroupId == groupId);
                if (scheduledTrack != null)
                {
                    _playbackScheduledTracks.Remove(scheduledTrack);
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<PlaybackScheduledTracks> GetDueTracks()
        {
            var time = DateTime.Now;
            var dueScheduledPlaybackTracks = new List<PlaybackScheduledTracks>();

            lock (_queueLock)
            {
                foreach (var playbackScheduledTrack in _playbackScheduledTracks)
                {
                    if (playbackScheduledTrack.DueTime <= time)
                    {
                        dueScheduledPlaybackTracks.Add(playbackScheduledTrack);
                    }
                }
            }

            if (dueScheduledPlaybackTracks.Count > 0)
            {
                lock (_queueLock)
                {
                    foreach (var dueScheduledPlaybackTrack in dueScheduledPlaybackTracks)
                    {
                        _playbackScheduledTracks.Remove(dueScheduledPlaybackTrack);
                    }
                }
            }

            return dueScheduledPlaybackTracks;
        }
    }
}