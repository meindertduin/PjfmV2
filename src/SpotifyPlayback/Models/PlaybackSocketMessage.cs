namespace SpotifyPlayback.Models
{
    public class PlaybackSocketMessage<T> : SocketMessage<T>
    {
        public PlaybackMessageContentType ContentType { get; set; }
    }
}