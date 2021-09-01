using System;
using Domain.SpotifyNummer;

namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class PlaybackGroupDto
    {
        public Guid GroupId { get; set; } 
        public string GroupName { get; set; } = null!;
        public SpotifyNummer? CurrentlyPlayingNummer { get; set; }
        public int ListenersCount { get; set; }
    }
}