using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.Extensions.Configuration;
using Pjfm.Application.GebruikerNummer.Models;
using Pjfm.Infrastructure;
using Pjfm.Infrastructure.Repositories;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Services
{
    public class PlaybackQueue : IPlaybackQueue
    {
        private readonly IConfiguration _configuration;
        private readonly LinkedList<SpotifyTrackDto> _fillerQueue = new();
        private readonly LinkedList<SpotifyTrackDto> _requestQueue = new();
        
        private readonly IEnumerable<string> _userIds = new List<string>();
        
        private TrackTerm _term = TrackTerm.Long;

        public PlaybackQueue(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<SpotifyTrackDto?> GetNextSpotifyTrack()
        {
            var getSpotifyTracksAmount = GetSpotifyTracksAmount();
            var spotifyTrackRepository = CreateSpotifyTrackRepository();

            var spotifyTracks = await spotifyTrackRepository.GetRandomUserSpotifyTracks(_userIds, new []{ _term }, getSpotifyTracksAmount);
            
            AddSpotifyTracksToQueue(spotifyTracks);

            return GetAndRemoveNextTrack();
        }

        public IEnumerable<SpotifyTrackDto> GetQueuedTracks(int amount)
        {
            var queuedTracks = _requestQueue.Take(amount).ToList();
            var leftOverAmount = amount - queuedTracks.Count();
            queuedTracks.AddRange(_fillerQueue.Take(leftOverAmount));
            
            return queuedTracks;
        }
        public void ResetQueue()
        {
            _fillerQueue.Clear();
        }

        public void SetTermijn(TrackTerm term)
        {
            _term = term;
        }

        public SpotifyTrackDto? AddTracksToQueue(IEnumerable<SpotifyTrackDto> tracks, SpotifyTrackDto? scheduledNextTrack)
        {
            ResetScheduledNextTrack(scheduledNextTrack);
            AddTracksToQueue(tracks);

            return GetAndRemoveNextTrack();
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
            if (_fillerQueue.Count == 0)
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
                _fillerQueue.AddLast(new SpotifyTrackDto()
                {
                    Title = spotifyTrack.Title,
                    Artists = spotifyTrack.Artists,
                    SpotifyTrackId = spotifyTrack.SpotifyTrackId,
                    TrackDurationMs = spotifyTrack.TrackDurationMs,
                    TrackTerm = spotifyTrack.TrackTerm,
                    TrackType = TrackType.Filler,
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

        private void ResetScheduledNextTrack(SpotifyTrackDto? scheduledNextTrack)
        {
            if (scheduledNextTrack != null)
            {
                switch (scheduledNextTrack.TrackType)
                {
                    case TrackType.Filler:
                        _fillerQueue.AddFirst(scheduledNextTrack);
                        break;
                    case TrackType.Request:
                        _requestQueue.AddFirst(scheduledNextTrack);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void AddTracksToQueue(IEnumerable<SpotifyTrackDto> tracks)
        {
            foreach (var track in tracks)
            {
                switch (track.TrackType)
                {
                    case TrackType.Filler:
                        _fillerQueue.AddLast(track);
                        break;
                    case TrackType.Request:
                        _requestQueue.AddLast(track);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private SpotifyTrackDto? GetAndRemoveNextTrack()
        {
            var request = _requestQueue.First?.Value;
            if (request != null)
            {
                _requestQueue.RemoveFirst();
                return request;
            }
            
            request = _fillerQueue.First?.Value;
            if (request != null)
            {
                _fillerQueue.RemoveFirst();
                return request;
            }

            return null;
        }
    }
}