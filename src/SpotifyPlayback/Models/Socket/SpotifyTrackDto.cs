using System;
using System.Collections.Generic;
using Domain.SpotifyTrack;

namespace SpotifyPlayback.Models.Socket
{
    public class SpotifyTrackDto
    {
        public string Title { get; set; } = null!;
        public string SpotifyTrackId { get; set; } = null!;
        public IEnumerable<string> Artists { get; set; } = null!;
        public TrackTerm TrackTerm { get; set; }
        public int TrackDurationMs { get; set; }
        public DateTime TrackStartDate { get; set; }
    }
}