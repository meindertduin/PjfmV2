using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Domain.SpotifyTrack;
using Pjfm.Application.Authentication;
using SpotifyAPI.Web;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyTrackService : ISpotifyTrackService
    {
        private readonly ISpotifyTrackRepository _spotifyTrackRepository;
        private readonly ISpotifyTokenService _spotifyTokenService;

        private const int TermijnSpotifyNummersAmount = 50;

        public SpotifyTrackService(ISpotifyTrackRepository spotifyTrackRepository, ISpotifyTokenService spotifyTokenService)
        {
            _spotifyTrackRepository = spotifyTrackRepository;
            _spotifyTokenService = spotifyTokenService;
        }

        public async Task UpdateUserSpotifyTracks(string userId)
        {
            var accessTokenResult = await _spotifyTokenService.GetUserSpotifyAccessToken(userId);
            var spotifyClient = new SpotifyClient(accessTokenResult.AccessToken);

            if (!accessTokenResult.IsSuccessful)
            {
                return;
            }

            var spotifyNummers = new List<SpotifyTrack>(150);

            foreach (var trackTermijn in Enum.GetValues<TrackTerm>())
            {
                var nummers = await GetTermTracks(ref spotifyClient, trackTermijn);
                
                spotifyNummers.AddRange(nummers.Items?.Select(s => new SpotifyTrack()
                {
                    Title = s.Name,
                    SpotifyTrackId = s.Id,
                    CreationDate = DateTime.Now,
                    Artists = s.Artists.Select(a => a.Name),
                    TrackTerm = trackTermijn,
                    TrackDurationMs = s.DurationMs,
                    UserId = userId,
                }) ?? Array.Empty<SpotifyTrack>());   
            }

            if (spotifyNummers.Count > 0)
            {
                await _spotifyTrackRepository.SetUserSpotifyTracks(spotifyNummers, userId);
            }
        }

        private Task<Paging<FullTrack>> GetTermTracks(ref SpotifyClient spotifyClient, TrackTerm term)
        {
            return spotifyClient.Personalization.GetTopTracks(new PersonalizationTopRequest()
            {
                TimeRangeParam = ConvertTermToTimeRange(term),
                Limit = TermijnSpotifyNummersAmount,
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
    }
}