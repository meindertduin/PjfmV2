using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.EntityFrameworkCore;
using Pjfm.Infrastructure;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyTrackService : ISpotifyTrackService
    {
        private readonly ISpotifyTrackRepository _spotifyTrackRepository;
        private readonly PjfmContext _pjfmContext;
        private readonly ISpotifyTrackClient _spotifyTrackClient;

        private const int RefreshLongTermTracksDays = 300;
        private const int RefreshLongMediumTracksDays = 100;
        private const int RefreshLongShortTracksDays = 10;
        private const int TermSpotifyTracksAmount = 50;

        public SpotifyTrackService(ISpotifyTrackRepository spotifyTrackRepository, PjfmContext pjfmContext,
            ISpotifyTrackClient spotifyTrackClient)
        {
            _spotifyTrackRepository = spotifyTrackRepository;
            _pjfmContext = pjfmContext;
            _spotifyTrackClient = spotifyTrackClient;
        }

        public async Task UpdateUserSpotifyTracks(string userId)
        {
            foreach (var trackTerm in Enum.GetValues<TrackTerm>())
            {
                var termExpiredTracks = await GetExpiredTracks(userId, trackTerm);

                if (termExpiredTracks.Count == 0)
                {
                    continue;
                }

                var tracksResult = await GetTermTracks(trackTerm, termExpiredTracks.Count, userId);
                var newTracks = GetNewTracks(userId, tracksResult, trackTerm);

                ReplaceExpiredTracks(termExpiredTracks, newTracks);
            }

            await _pjfmContext.SaveChangesAsync();
        }

        public async Task SetUserSpotifyTracks(string userId)
        {
            var spotifyTracks = new List<SpotifyTrack>(150);

            foreach (var trackTerm in Enum.GetValues<TrackTerm>())
            {
                var tracksResult = await GetTermTracks(trackTerm, TermSpotifyTracksAmount, userId);
                spotifyTracks.AddRange(tracksResult.Items?.Select(s => new SpotifyTrack()
                {
                    Title = s.Name,
                    SpotifyTrackId = s.Id,
                    CreationDate = DateTime.Now,
                    Artists = s.Artists.Select(a => a.Name),
                    TrackTerm = trackTerm,
                    TrackDurationMs = s.DurationMs,
                    UserId = userId,
                    SpotifyAlbum = new SpotifyAlbum()
                    {
                        AlbumId = s.Album.Id,
                        Title = s.Album.Name,
                        AlbumImage =  new SpotifyAlbumImage()
                        {
                           Url = s.Album.Images.FirstOrDefault(x => x.Width == 300)?.Url ?? string.Empty,
                           Height = s.Album.Images.FirstOrDefault(x => x.Width == 300)?.Height ?? 0,
                           Width = s.Album.Images.FirstOrDefault(x => x.Width == 300)?.Height ?? 0,
                        },
                        // retrieve this from the release date
                        ReleaseDate = DateTime.Now,
                    }
                }).ToArray() ?? Array.Empty<SpotifyTrack>());
            }

            if (spotifyTracks.Count > 0)
            {
                await _spotifyTrackRepository.SetUserSpotifyTracks(spotifyTracks, userId);
            }
        }

        private async Task<List<SpotifyTrack>> GetExpiredTracks(string userId, TrackTerm trackTerm)
        {
            var refreshDays = GetTermTracksRefreshDays(trackTerm);
            var beforeDate = DateTime.Now.Subtract(TimeSpan.FromDays(refreshDays));
            var termExpiredTracks = await _pjfmContext.SpotifyTracks
                .Where(x => x.UserId == userId)
                .Where(x => x.TrackTerm == trackTerm)
                .Where(x => x.CreationDate <= beforeDate)
                .ToListAsync();

            return termExpiredTracks;
        }

        private static SpotifyTrack[] GetNewTracks(string userId, SpotifyTracksResult tracksResult, TrackTerm trackTerm)
        {
            if (tracksResult.Items == null)
            {
                throw new NullReferenceException();
            }

            var newTracks = tracksResult.Items.Select(s => new SpotifyTrack()
            {
                Title = s.Name,
                SpotifyTrackId = s.Id,
                CreationDate = DateTime.Now,
                Artists = s.Artists.Select(a => a.Name),
                TrackTerm = trackTerm,
                TrackDurationMs = s.DurationMs,
                UserId = userId,
            }).ToArray();
            return newTracks;
        }

        private void ReplaceExpiredTracks(List<SpotifyTrack> termExpiredTracks, SpotifyTrack[] newTracks)
        {
            for (int i = 0; i < termExpiredTracks.Count; i++)
            {
                var expiredTrack = termExpiredTracks[i];
                var newTrack = newTracks[i];

                expiredTrack.CreationDate = newTrack.CreationDate;
                expiredTrack.Title = newTrack.Title;
                expiredTrack.Artists = newTrack.Artists;
                expiredTrack.SpotifyTrackId = newTrack.SpotifyTrackId;
                expiredTrack.TrackDurationMs = newTrack.TrackDurationMs;
            }
        }

        private int GetTermTracksRefreshDays(TrackTerm term)
        {
            switch (term)
            {
                case TrackTerm.Long:
                    return RefreshLongTermTracksDays;
                case TrackTerm.Medium:
                    return RefreshLongMediumTracksDays;
                case TrackTerm.Short:
                    return RefreshLongShortTracksDays;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Task<SpotifyTracksResult> GetTermTracks(TrackTerm term, int amount, string userId)
        {
            return _spotifyTrackClient.GetSpotifyTracks(new SpotifyTrackRequest()
            {
                TrackTerm = term, PageSize = amount
            }, userId);
        }
    }

    public interface ISpotifyTrackService
    {
        Task UpdateUserSpotifyTracks(string userId);
        Task SetUserSpotifyTracks(string userId);
    }
}