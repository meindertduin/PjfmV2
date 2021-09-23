
namespace Domain.SpotifyTrack
{
    public class SpotifyAlbum
    {
        public int Id { get; set; }
        public string AlbumId { get; set; } = null!;
        public SpotifyAlbumImage AlbumImage { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ReleaseDate { get; set; } = null!;
        public int SpotifyTrackId { get; set; }
        public SpotifyTrack SpotifyTrack { get; set; } = null!;
    }
}