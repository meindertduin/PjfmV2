namespace SpotifyPlayback.Models
{
    public enum MessageType
    {
        PlaybackInfo = 0,
        ConnectionEstablished = 100,
        ConnectionClosed = 101,
        JoinedGroupStatusUpdate = 200,
    }
}