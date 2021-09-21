using System;
using System.Collections.Generic;
using Domain.SpotifyTrack;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyTracksResult
    {
        public IEnumerable<SpotifyTrackItemResult> Items { get; set; } = null!;
        public int Total { get; set; }
    }

    public class SpotifyTrackItemResult
    {
        public IEnumerable<SpotifyTrackArtistResult> Artists { get; set; } = null!;
        public SpotifyAlbumResult Album { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int DurationMs { get; set; }
        public string Id { get; set; } = null!;
        public TrackTerm TrackTerm { get; set; }
    }

    public class SpotifyAlbumResult
    {
        public string Id { get; set; } = null!;
        public IEnumerable<SpotifyTrackImageResult> Images { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
    }

    public class SpotifyTrackArtistResult
    {
        public string Name { get; set; } = null!;
    }

    public class SpotifyTrackImageResult
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string Url { get; set; } = null!;
    }
}