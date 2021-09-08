using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.Extensions.Configuration;
using Pjfm.Infrastructure;
using Pjfm.Infrastructure.Repositories;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Services
{
    public class PlaybackQueue : IPlaybackQueue
    {
        private readonly ISpotifyTrackRepository _spotifyTrackRepository;
        private readonly IConfiguration _configuration;
        private Queue<SpotifyTrackDto> _spotifyTracks = new();
        private IEnumerable<string> _userIds = new List<string>();
        
        private TrackTerm _term = TrackTerm.Long;

        public PlaybackQueue(ISpotifyTrackRepository spotifyTrackRepository, IConfiguration configuration)
        {
            _spotifyTrackRepository = spotifyTrackRepository;
            _configuration = configuration;
        }
        public async Task<SpotifyTrackDto> GetNextSpotifyTrack()
        {
            int getSpotifyTracksAmount = 1;
            if (_spotifyTracks.Count == 0)
            {
                getSpotifyTracksAmount = 20;
            }

            var connectionString = _configuration.GetValue<string>("ConnectionStrings:ApplicationDb");
            var spotifyTrackRepository = new SpotifyTrackRepository(PjfmContextFactory.Create(connectionString));
            
            var spotifyTracks =
                await spotifyTrackRepository.GetRandomUserSpotifyTracks(_userIds, new []{ _term }, getSpotifyTracksAmount);
            
            foreach (var spotifyTrack in spotifyTracks)
            {
                _spotifyTracks.Enqueue(new SpotifyTrackDto()
                {
                    Title = spotifyTrack.Title,
                    Artists = spotifyTrack.Artists,
                    SpotifyTrackId = spotifyTrack.SpotifyTrackId,
                    TrackDurationMs = spotifyTrack.TrackDurationMs,
                    TrackTerm = spotifyTrack.TrackTerm,
                });
            }

            return _spotifyTracks.Dequeue();
        }

        public void ResetQueue()
        {
            _spotifyTracks.Clear();
        }

        public void SetTermijn(TrackTerm term)
        {
            _term = term;
        }
    }
}