namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class SpotifyPlayRequestDto
    {
        public string ContextUri { get; set; } = null!;
        public string[] Uris { get; set; } = null!;
        public object Offset { get; set; } = null!;
        public int PositionMs { get; set; }
    }
}