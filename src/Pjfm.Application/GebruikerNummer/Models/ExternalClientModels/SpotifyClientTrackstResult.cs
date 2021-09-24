using System.Collections.Generic;
using System.Linq;
using Domain.SpotifyTrack;
using Pjfm.Application.GebruikerNummer.Models;

namespace Pjfm.Application.GebruikerNummer
{
    public class SpotifyClientTrackstResult
    {
        public IEnumerable<SpotifyClientTrackItemResult> Items { get; set; } = null!;
        public int Total { get; set; }
    }

    public class SpotifyClientTrackItemResult
    {
        public IEnumerable<SpotifyClientTrackArtistResult> Artists { get; set; } = null!;
        public SpotifyClientAlbumResult Album { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int DurationMs { get; set; }
        public string Id { get; set; } = null!;
        public TrackTerm TrackTerm { get; set; }

        public SpotifyTrackDto GetTrackDto(int albumImageWidth)
        {
            var rightSizeAlbumImage = Album.Images.FirstOrDefault(x => x.Width == albumImageWidth);
            
            return new SpotifyTrackDto()
            {
                Title = Name,
                SpotifyTrackId = Id,
                Artists = Artists.Select(a => a.Name),
                TrackTerm = TrackTerm.None,
                TrackDurationMs = DurationMs,
                SpotifyAlbum = new SpotifyAlbumDto()
                {
                    AlbumId = Album.Id,
                    Title = Album.Name,
                    AlbumImage = new SpotifyAlbumImageDto()
                    {
                        Url = rightSizeAlbumImage?.Url ?? string.Empty,
                        Width = rightSizeAlbumImage?.Width ?? 0,
                        Height = rightSizeAlbumImage?.Height ?? 0,
                    },
                    ReleaseDate = Album.ReleaseDate,
                }
            };
        }
    }

    public class SpotifyClientAlbumResult
    {
        public string Id { get; set; } = null!;
        public IEnumerable<SpotifyClientTrackImageResult> Images { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ReleaseDate { get; set; }
    }

    public class SpotifyClientTrackArtistResult
    {
        public string Name { get; set; } = null!;
    }

    public class SpotifyClientTrackImageResult
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string Url { get; set; } = null!;
    }
}