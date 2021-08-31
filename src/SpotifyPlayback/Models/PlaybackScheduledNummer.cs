using System;
using Domain.SpotifyNummer;

namespace SpotifyPlayback.Models
{
    public class PlaybackScheduledNummer
    {
        public Guid GroupId { get; set; }
        public SpotifyNummer SpotifyNummer { get; set; } = null!;
        public DateTime DueTime { get; set; }
    }
}