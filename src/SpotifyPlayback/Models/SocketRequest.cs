namespace SpotifyPlayback.Models
{
    public class SocketRequest<T>
    {
        public RequestType RequestType { get; set; }
        public T Body { get; set; }
    }
}