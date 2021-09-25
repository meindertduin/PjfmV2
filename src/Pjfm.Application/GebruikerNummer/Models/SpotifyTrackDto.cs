using System;
using System.Collections.Generic;
using Domain.SpotifyTrack;
using Pjfm.Application.ApplicationUser;

namespace Pjfm.Application.GebruikerNummer.Models
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
        public TrackType TrackType { get; set; }
        public ApplicationUserDto User { get; set; } = null!;
    }

    public enum TrackType
    {
        Request,
        Filler,
        ModRequest,
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