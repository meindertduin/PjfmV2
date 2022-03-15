using System;
using Pjfm.Application.GebruikerNummer.Models;

namespace SpotifyPlayback.Models
{
    public class PlaybackScheduledTrack
    {
        public string GroupId { get; set; }
        public SpotifyTrackDto SpotifyTrack { get; set; } = null!;
        public DateTime DueTime { get; set; }
    }
}