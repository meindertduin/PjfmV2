using System.Collections.Generic;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyTrackSearchClientResult
    {
        public SpotifyTracksTracksResult Tracks{ get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public int RetrievedAmount => Limit > Total ? Total : Limit;
    }

    public class SpotifyTracksTracksResult
    {
        public IEnumerable<SpotifyClientTrackItemResult> Items { get; set; } = null!;
    }
}