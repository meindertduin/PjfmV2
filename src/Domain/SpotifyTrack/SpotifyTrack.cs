using System;
using System.Collections.Generic;

namespace Domain.SpotifyTrack
{
    public class SpotifyTrack : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string SpotifyTrackId { get; set; } = null!;
        public IEnumerable<string> Artists { get; set; } = null!;
        public TrackTerm TrackTerm { get; set; }
        public int TrackDurationMs { get; set; }
        public DateTime CreationDate { get; set; }
    }
}