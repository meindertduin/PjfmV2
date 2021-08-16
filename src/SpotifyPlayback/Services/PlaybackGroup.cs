using System.Threading.Tasks;
using Domain.SpotifyNummer;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Services
{
    public class PlaybackGroup : IPlaybackGroup
    {
        private readonly IPlaybackQueue _playbackQueue;
        private SpotifyNummer? _currentlyPlayingNumber = null;

        public PlaybackGroup(IPlaybackQueue playbackQueue)
        {
            _playbackQueue = playbackQueue;
        }
        
        public async Task<SpotifyNummer> GetNextNummer()
        {
            var newNummer = await _playbackQueue.GetNextSpotifyNummer();
            _currentlyPlayingNumber = newNummer;

            return newNummer;
        }
    }
}