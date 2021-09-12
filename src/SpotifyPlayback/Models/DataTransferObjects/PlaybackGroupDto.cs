using System;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class PlaybackGroupDto
    {
        public Guid GroupId { get; set; } 
        public string GroupName { get; set; } = null!;
        public SpotifyTrackDto? CurrentlyPlayingTrack { get; set; }
        public int ListenersCount { get; set; }
    }
}