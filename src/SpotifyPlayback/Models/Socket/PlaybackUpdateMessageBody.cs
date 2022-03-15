using System;
using System.Collections.Generic;
using Pjfm.Application.GebruikerNummer.Models;

namespace SpotifyPlayback.Models.Socket
{
    public class PlaybackUpdateMessageBody
    {
        public string GroupId { get; set; } 
        public string GroupName { get; set; } = null!;
        public SpotifyTrackDto? CurrentlyPlayingTrack { get; set; }
        public IEnumerable<SpotifyTrackDto> QueuedTracks { get; set; } = null!;
    }
}