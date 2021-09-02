using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.Extensions.Configuration;
using Pjfm.Infrastructure;
using Pjfm.Infrastructure.Repositories;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Services
{
    public class PlaybackQueue : IPlaybackQueue
    {
        private readonly ISpotifyTrackRepository _spotifyTrackRepository;
        private readonly IConfiguration _configuration;
        private Queue<SpotifyTrack> _spotifyNummers = new();
        private IEnumerable<string> _gebruikerIds = new List<string>();
        
        private TrackTerm _term = TrackTerm.Long;

        public PlaybackQueue(ISpotifyTrackRepository spotifyTrackRepository, IConfiguration configuration)
        {
            _spotifyTrackRepository = spotifyTrackRepository;
            _configuration = configuration;
        }
        public async Task<SpotifyTrack> GetNextSpotifyNummer()
        {
            int getSpotifyNummersAmount = 1;
            if (_spotifyNummers.Count == 0)
            {
                getSpotifyNummersAmount = 20;
            }

            var connectionString = _configuration.GetValue<string>("ConnectionStrings:ApplicationDb");
            var spotifyNummerRepository = new SpotifyTrackRepository(PjfmContextFactory.Create(connectionString));
            
            var spotifyNummers =
                await spotifyNummerRepository.GetRandomUserSpotifyTracks(_gebruikerIds, new []{ _term }, getSpotifyNummersAmount);
            
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

        public void SetTermijn(TrackTerm term)
        {
            _term = term;
        }
    }
}