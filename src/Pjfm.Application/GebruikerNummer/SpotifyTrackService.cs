using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Microsoft.EntityFrameworkCore;
using Pjfm.Application.Authentication;
using Pjfm.Infrastructure;
using SpotifyAPI.Web;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyTrackService : ISpotifyTrackService
    {
        private readonly ISpotifyTrackRepository _spotifyTrackRepository;
        private readonly ISpotifyTokenService _spotifyTokenService;
        private readonly PjfmContext _pjfmContext;

        private const int RefreshLongTermTracksDays = 300;
        private const int RefreshLongMediumTracksDays = 100;
        private const int RefreshLongShortTracksDays = 10;
        private const int TermSpotifyTracksAmount = 50;

        public SpotifyTrackService(ISpotifyTrackRepository spotifyTrackRepository,ISpotifyTokenService spotifyTokenService, PjfmContext pjfmContext)
        {
            _spotifyTrackRepository = spotifyTrackRepository;
            _spotifyTokenService = spotifyTokenService;
            _pjfmContext = pjfmContext;
        }

        public async Task UpdateUserSpotifyTracks(string userId)
        {
            var spotifyClient = await CreateSpotifyClient(userId);
            
            foreach (var trackTerm in Enum.GetValues<TrackTerm>())
            {
                var termExpiredTracks = await GetExpiredTracks(userId, trackTerm);

                if (termExpiredTracks.Count == 0)
                {
                    continue;
                }

                var tracksResult = await GetTermTracks(ref spotifyClient, trackTerm, termExpiredTracks.Count);
                var newTracks = GetNewTracks(userId, tracksResult, trackTerm);
                
                ReplaceExpiredTracks(termExpiredTracks, newTracks);
            }

            await _pjfmContext.SaveChangesAsync();
        }
        public async Task SetUserSpotifyTracks(string userId)
        {
            var spotifyClient = await CreateSpotifyClient(userId);
            var spotifyTracks = new List<SpotifyTrack>(150);
            
            foreach (var trackTerm in Enum.GetValues<TrackTerm>())
            {
                var tracksResult = await GetTermTracks(ref spotifyClient, trackTerm, TermSpotifyTracksAmount);
                spotifyTracks.AddRange(tracksResult.Items?.Select(s => new SpotifyTrack()
                {
                    Title = s.Name,
                    SpotifyTrackId = s.Id,
                    CreationDate = DateTime.Now,
                    Artists = s.Artists.Select(a => a.Name),
                    TrackTerm = trackTerm,
                    TrackDurationMs = s.DurationMs,
                    UserId = userId,
                }).ToArray() ?? Array.Empty<SpotifyTrack>());
            }
            
            await _spotifyTrackRepository.SetUserSpotifyTracks(spotifyTracks, userId);
        }


        private async Task<SpotifyClient> CreateSpotifyClient(string userId)
        {
            var accessTokenResult = await _spotifyTokenService.GetUserSpotifyAccessToken(userId);

            if (!accessTokenResult.IsSuccessful)
            {
                throw new NullReferenceException();
            }
            
            return new SpotifyClient(accessTokenResult.AccessToken);
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

        private static SpotifyTrack[] GetNewTracks(string userId, Paging<FullTrack> tracksResult, TrackTerm trackTerm)
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

        private Task<Paging<FullTrack>> GetTermTracks(ref SpotifyClient spotifyClient, TrackTerm term, int amount)
        {
            return spotifyClient.Personalization.GetTopTracks(new PersonalizationTopRequest()
            {
                TimeRangeParam = ConvertTermToTimeRange(term),
                Limit = amount,
            });
        }
        private PersonalizationTopRequest.TimeRange ConvertTermToTimeRange(TrackTerm term)
        {
            switch (term)
            {
                case TrackTerm.Short:
                    return PersonalizationTopRequest.TimeRange.ShortTerm;
                case TrackTerm.Medium:
                    return PersonalizationTopRequest.TimeRange.MediumTerm;
                case TrackTerm.Long:
                    return PersonalizationTopRequest.TimeRange.LongTerm;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }

    public interface ISpotifyTrackService
    {
        Task UpdateUserSpotifyTracks(string userId);
        Task SetUserSpotifyTracks(string userId);
    }
}