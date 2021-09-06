using System;
using Domain.SpotifyTrack;

namespace SpotifyPlayback.Models
{
    public class PlaybackScheduledTrack
    {
        public Guid GroupId { get; set; }
        public SpotifyTrack SpotifyTrack { get; set; } = null!;
        public DateTime DueTime { get; set; }
    }
}