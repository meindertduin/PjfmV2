
using System.Threading.Tasks;

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
            return new ()
            {
                IsSuccessful = false,
                Message = message,
            };
        }

        public static PlaybackRequestResult<T> Success<T>(T result, string message)
        {
            return new ()
            {
                IsSuccessful = true,
                Message = message,
            };
        }

        public static Task<PlaybackRequestResult<T>> SuccessAsync<T>(T result, string message)
        {
            return Task.FromResult(Success(result, message));
        }

        public static Task<PlaybackRequestResult<T>> FailAsync<T>(string message)
        {
            return Task.FromResult(Fail<T>(message));
        }
    }
}