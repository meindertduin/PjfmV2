namespace Domain.SpotifyTrack
{
    public class SpotifyAlbumImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
        public SpotifyAlbum SpotifyAlbum { get; set; } = null!;
        public int AlbumId { get; set; }
    }
}