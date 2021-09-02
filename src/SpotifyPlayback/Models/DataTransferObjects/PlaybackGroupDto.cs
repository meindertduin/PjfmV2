using System;
using Domain.SpotifyTrack;

namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class PlaybackGroupDto
    {
        public Guid GroupId { get; set; } 
        public string GroupName { get; set; } = null!;
        public SpotifyTrack? CurrentlyPlayingTrack { get; set; }
        public int ListenersCount { get; set; }
    }
}