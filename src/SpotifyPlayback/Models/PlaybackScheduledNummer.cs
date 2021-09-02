using System;
using Domain.SpotifyTrack;

namespace SpotifyPlayback.Models
{
    public class PlaybackScheduledNummer
    {
        public Guid GroupId { get; set; }
        public SpotifyTrack SpotifyTrack { get; set; } = null!;
        public DateTime DueTime { get; set; }
    }
}