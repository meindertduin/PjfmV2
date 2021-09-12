using System;
using Domain.SpotifyTrack;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Models
{
    public class PlaybackScheduledTrack
    {
        public Guid GroupId { get; set; }
        public SpotifyTrackDto SpotifyTrack { get; set; } = null!;
        public DateTime DueTime { get; set; }
    }
}