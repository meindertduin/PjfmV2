using System;
using System.Collections.Generic;
using Pjfm.Application.GebruikerNummer.Models;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class PlaybackGroupDto
    {
        public string GroupId { get; set; } 
        public string GroupName { get; set; } = null!;
        public SpotifyTrackDto? CurrentlyPlayingTrack { get; set; }
        public IEnumerable<SpotifyTrackDto> QueuedTracks { get; set; } = null!;
        public int ListenersCount { get; set; }
    }
}