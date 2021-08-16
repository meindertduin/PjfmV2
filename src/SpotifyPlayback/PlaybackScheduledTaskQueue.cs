
using SpotifyPlayback.Models;

namespace SpotifyPlayback
{
    public class PlaybackScheduledTaskQueue
    {
        private const int DefaultCapacity = 5;
        
        private object _queueLock = new object();
        private PlaybackScheduledNummer[] _playbackScheduledNummers;

        public PlaybackScheduledTaskQueue()
        {
            _playbackScheduledNummers = new PlaybackScheduledNummer[DefaultCapacity];
        }
        
        public PlaybackScheduledTaskQueue(int capacity)
        {
            _playbackScheduledNummers = new PlaybackScheduledNummer[capacity];
        }

        public void AddPlaybackScheduledNummer()
        {
            _playbackScheduledNummers.Length
        }
    }
}