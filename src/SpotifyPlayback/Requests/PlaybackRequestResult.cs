
namespace SpotifyPlayback.Requests
{
    public class PlaybackRequestResult<T>
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = null!;
        public T Result { get; set; } = default!;
    }

    public static class PlaybackRequestResult
    {
        public static PlaybackRequestResult<T> Fail<T>(string message)
        {
            return new PlaybackRequestResult<T>()
            {
                IsSuccessful = false,
                Message = message,
            };
        }

        public static PlaybackRequestResult<T> Success<T>(T result, string message)
        {
            return new PlaybackRequestResult<T>()
            {
                IsSuccessful = true,
                Message = message,
            };
        }
    }
}