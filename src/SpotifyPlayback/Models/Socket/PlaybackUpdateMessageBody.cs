using System;
using System.Collections.Generic;

namespace SpotifyPlayback.Models.Socket
{
    public class PlaybackUpdateMessageBody
    {
        public Guid GroupId { get; set; } 
        public string GroupName { get; set; } = null!;
        public SpotifyTrackDto? CurrentlyPlayingTrack { get; set; }
        public IEnumerable<SpotifyTrackDto> QueuedTracks { get; set; } = null!;
    }
}