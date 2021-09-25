using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pjfm.Application.GebruikerNummer.Models;
using Pjfm.Common;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroup : IPlaybackGroup
    {
        private readonly IPlaybackQueue _playbackQueue;
        private SpotifyTrackDto? _currentlyPlayingTrack = null;
        private SpotifyTrackDto? _nextTrack = null;

        private List<Guid> _joinedConnections = new();
        private List<ListenerDto> _listeners = new();
        private readonly object listenerLock = new();
        private readonly object joinedConnectionsLock = new();

        public Guid GroupId { get; private set; }
        public string GroupName { get; private set; }

        public PlaybackGroup(IPlaybackQueue playbackQueue, Guid groupId, string groupName)
        {
            _playbackQueue = playbackQueue;
            GroupName = groupName;
            GroupId = groupId;
        }

        public async Task<SpotifyTrackDto> GetNextTrack()
        {
            var newTrack = await _playbackQueue.GetNextSpotifyTrack();
            
            Guard.NotNull(newTrack, nameof(newTrack));
            
            SetCurrentAndNextTrack(newTrack);
            SetCurrentlyPlayingTrackStartTime();

            return newTrack;
        }

        private void SetCurrentAndNextTrack(SpotifyTrackDto? newTrack)
        {
            if (_currentlyPlayingTrack == null)
            {
                _currentlyPlayingTrack = newTrack;
            }
            else if (_nextTrack == null)
            {
                _nextTrack = newTrack;
            }
            else
            {
                _currentlyPlayingTrack = _nextTrack;
                _nextTrack = newTrack;
            }
        }

        private void SetCurrentlyPlayingTrackStartTime()
        {
            if (_currentlyPlayingTrack != null)
            {
                _currentlyPlayingTrack.TrackStartDate = DateTime.Now;
            }
        }

        public IEnumerable<ListenerDto> GetGroupListeners()
        {
            return _listeners;
        }

        public IEnumerable<Guid> GetJoinedConnectionIds()
        {
            return _joinedConnections;
        }

        public bool AddListener(ListenerDto listener)
        {
            if (!_listeners.Contains(listener))
            {
                lock (listenerLock)
                {
                    _listeners.Add(listener);
                }

                return true;
            }

            return false;
        }
        public bool AddJoinedConnectionId(Guid connectionId)
        {
            if (!_joinedConnections.Contains(connectionId))
            {
                lock (joinedConnectionsLock)
                {
                    _joinedConnections.Add(connectionId);
                }

                return true;
            }

            return false;
        }

        public bool RemoveJoinedConnection(Guid connectionId)
        {
            lock (joinedConnectionsLock)
            {
                return _joinedConnections.Remove(connectionId);
            }
        }

        public bool RemoveListener(Guid connectionId)
        {
            lock (listenerLock)
            {
                var listener = _listeners.FirstOrDefault(x => x.ConnectionId == connectionId);
                if (listener != null)
                {
                    _listeners.Remove(listener);
                    return true;
                }
            }

            return false;
        }

        public bool ContainsListeners(ListenerDto listener)
        {
            return _listeners.Contains(listener);
        }

        public bool ContainsJoinedConnectionId(Guid connectionId)
        {
            return _joinedConnections.Contains(connectionId);
        }

        public bool HasListeners()
        {
            return !_listeners.Any();
        }

        public PlaybackGroupDto GetPlaybackGroupInfo()
        {
            var queuedTracks = GetQueuedTracks();

            _playbackQueue.GetQueuedTracks(9);

            return new()
            {
                GroupId = GroupId,
                GroupName = GroupName,
                ListenersCount = _listeners.Count,
                CurrentlyPlayingTrack = _currentlyPlayingTrack,
                QueuedTracks = queuedTracks,
            };
        }

        public void AddTracksToQueue(IEnumerable<SpotifyTrackDto> tracks)
        {
            _nextTrack = _playbackQueue.AddTracksToQueue(tracks, _nextTrack);
        }

        private List<SpotifyTrackDto> GetQueuedTracks()
        {
            var queuedTracks = new List<SpotifyTrackDto>();

            if (_nextTrack != null)
            {
                queuedTracks.Add(_nextTrack);
                queuedTracks.AddRange(_playbackQueue.GetQueuedTracks(9));
            }

            return queuedTracks;
        }
    }
}