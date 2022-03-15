using System;
using System.Collections.Generic;
using System.Linq;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback
{
    public class PlaybackScheduledTrackQueue : IPlaybackScheduledTrackQueue
    {
        private object _queueLock = new ();
        private List<PlaybackScheduledTrack> _playbackScheduledTracks = new();
        public int Count => _playbackScheduledTracks.Count;

        public void AddPlaybackScheduledTrack(PlaybackScheduledTrack playbackScheduledTrack)
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

        public IEnumerable<PlaybackScheduledTrack> GetScheduledTracks()
        {
            lock (_queueLock)
            {
                return _playbackScheduledTracks;
            }
        }

        public bool RemovePlaybackScheduledTrack(string groupId)
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

        public IEnumerable<PlaybackScheduledTrack> GetDueTracks()
        {
            var time = DateTime.Now;
            var dueScheduledPlaybackTracks = new List<PlaybackScheduledTrack>();

            lock (_queueLock)
            {
                var dueTracks = _playbackScheduledTracks.Where(playbackScheduledTrack => playbackScheduledTrack.DueTime <= time);
                dueScheduledPlaybackTracks.AddRange(dueTracks);
            }

            if (dueScheduledPlaybackTracks.Count <= 0)
            {
                return dueScheduledPlaybackTracks;
            }
            
            lock (_queueLock)
            {
                foreach (var dueScheduledPlaybackTrack in dueScheduledPlaybackTracks)
                {
                    _playbackScheduledTracks.Remove(dueScheduledPlaybackTrack);
                }
            }

            return dueScheduledPlaybackTracks;
        }
    }
}