using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyNummer;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Services
{
    public class PlaybackQueue : IPlaybackQueue
    {
        private readonly ISpotifyNummerRepository _spotifyNummerRepository;
        private Queue<SpotifyNummer> _spotifyNummers = new();
        private IEnumerable<string> _gebruikerIds = new List<string>();
        
        private TrackTermijn _termijn = TrackTermijn.Lang;

        public PlaybackQueue(ISpotifyNummerRepository spotifyNummerRepository)
        {
            _spotifyNummerRepository = spotifyNummerRepository;
        }
        public async Task<SpotifyNummer> GetNextSpotifyNummer()
        {
            int getSpotifyNummersAmount = 1;
            if (_spotifyNummers.Count == 0)
            {
                getSpotifyNummersAmount = 20;
            }
            
            var spotifyNummers =
                await _spotifyNummerRepository.GetRandomGebruikersSpotifyNummers(_gebruikerIds, new []{ _termijn }, getSpotifyNummersAmount);
            foreach (var spotifyNummer in spotifyNummers)
            {
                _spotifyNummers.Enqueue(spotifyNummer);
            }

            return _spotifyNummers.Dequeue();
        }

        public void ResetQueue()
        {
            _spotifyNummers.Clear();
        }

        public void SetTermijn(TrackTermijn termijn)
        {
            _termijn = termijn;
        }
    }
}