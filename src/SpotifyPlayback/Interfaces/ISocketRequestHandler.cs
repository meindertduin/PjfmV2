using System.Threading.Tasks;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface ISocketRequestHandler
    {
        public Task HandleSocketRequest(byte[] buffer, SocketConnection socketConnection);
    }
}