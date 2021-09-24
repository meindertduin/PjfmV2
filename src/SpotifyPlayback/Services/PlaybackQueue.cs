using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.Extensions.Configuration;
using Pjfm.Application.GebruikerNummer.Models;
using Pjfm.Infrastructure;
using Pjfm.Infrastructure.Repositories;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Services
{
    public class PlaybackQueue : IPlaybackQueue
    {
        private readonly IConfiguration _configuration;
        private Queue<SpotifyTrackDto> _spotifyTracks = new();
        private IEnumerable<string> _userIds = new List<string>();
        
        private TrackTerm _term = TrackTerm.Long;

        public PlaybackQueue(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<SpotifyTrackDto> GetNextSpotifyTrack()
        {
            var getSpotifyTracksAmount = GetSpotifyTracksAmount();
            var spotifyTrackRepository = CreateSpotifyTrackRepository();

            var spotifyTracks = await spotifyTrackRepository.GetRandomUserSpotifyTracks(_userIds, new []{ _term }, getSpotifyTracksAmount);
            
            AddSpotifyTracksToQueue(spotifyTracks);

            return _spotifyTracks.Dequeue();
        }

        public IEnumerable<SpotifyTrackDto> GetQueuedTracks(int amount)
        {
            return _spotifyTracks.Take(amount);
        }

        private SpotifyTrackRepository CreateSpotifyTrackRepository()
        {
            var connectionString = _configuration.GetValue<string>("ConnectionStrings:ApplicationDb");
            var spotifyTrackRepository = new SpotifyTrackRepository(PjfmContextFactory.Create(connectionString));
            return spotifyTrackRepository;
        }

        private int GetSpotifyTracksAmount()
        {
            int getSpotifyTracksAmount = 1;
            if (_spotifyTracks.Count == 0)
            {
                getSpotifyTracksAmount = 20;
            }

            return getSpotifyTracksAmount;
        }
        
        private void AddSpotifyTracksToQueue(List<SpotifyTrack> spotifyTracks)
        {
            foreach (var spotifyTrack in spotifyTracks)
            {
                // TODO: add a mapping
                _spotifyTracks.Enqueue(new SpotifyTrackDto()
                {
                    Title = spotifyTrack.Title,
                    Artists = spotifyTrack.Artists,
                    SpotifyTrackId = spotifyTrack.SpotifyTrackId,
                    TrackDurationMs = spotifyTrack.TrackDurationMs,
                    TrackTerm = spotifyTrack.TrackTerm,
                    SpotifyAlbum = new SpotifyAlbumDto()
                    {
                        AlbumId = spotifyTrack.SpotifyAlbum.AlbumId,
                        Title = spotifyTrack.SpotifyAlbum.Title,
                        ReleaseDate = spotifyTrack.SpotifyAlbum.ReleaseDate,
                        
                        AlbumImage = new SpotifyAlbumImageDto()
                        {
                            Width = spotifyTrack.SpotifyAlbum.AlbumImage.Width,
                            Height = spotifyTrack.SpotifyAlbum.AlbumImage.Height,
                            Url = spotifyTrack.SpotifyAlbum.AlbumImage.Url,
                        }
                    }
                });
            }
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