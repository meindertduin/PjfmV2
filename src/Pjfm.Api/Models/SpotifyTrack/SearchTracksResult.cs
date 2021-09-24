using System.Collections.Generic;
using Pjfm.Application.GebruikerNummer.Models;

namespace Pjfm.Api.Models.SpotifyTrack
{
    public class SearchTracksResult
    {
        public IEnumerable<SpotifyTrackDto> Tracks { get; set; } = null!;
    }
}