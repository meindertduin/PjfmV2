using System;
using System.Collections.Generic;

namespace Domain.SpotifyTrack
{
    public class SpotifyAlbum
    {
        public int Id { get; set; }
        public string AlbumId { get; set; } = null!;
        public ICollection<SpotifyAlbumImage> AlbumImages { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public ICollection<SpotifyTrack> SpotifyTracks { get; set; } = null!;
    }
}