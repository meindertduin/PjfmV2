using System;
using Domain.SpotifyTrack;

namespace SpotifyPlayback.Models.Socket
{
    public class PlaybackUpdateMessageBody
    {
        public Guid GroupId { get; set; } 
        public string GroupName { get; set; } = null!;
        public SpotifyTrack? CurrentlyPlayingTrack { get; set; }
    }
}