using System.Collections.Generic;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyClientGetTracksResult
    {
        public IEnumerable<SpotifyClientTrackItemResult> Tracks { get; set; } = null!;
    }
}