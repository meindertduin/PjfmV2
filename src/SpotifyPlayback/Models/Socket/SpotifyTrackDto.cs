using System;
using System.Collections.Generic;
using Domain.SpotifyTrack;

namespace SpotifyPlayback.Models.Socket
{
    public class SpotifyTrackDto
    {
        public string Title { get; set; } = null!;
        public string SpotifyTrackId { get; set; } = null!;
        public IEnumerable<string> Artists { get; set; } = null!;
        public TrackTerm TrackTerm { get; set; }
        public int TrackDurationMs { get; set; }
        public DateTime TrackStartDate { get; set; }
        public SpotifyAlbumDto SpotifyAlbum { get; set; } = null!;
    }

    public class SpotifyAlbumDto
    {
        public string AlbumId { get; set; } = null!;
        public SpotifyAlbumImageDto AlbumImage { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ReleaseDate { get; set; } = null!;
    }

    public class SpotifyAlbumImageDto
    {
        public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
    }
}