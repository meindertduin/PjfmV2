using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Services
{
    public class SpotifyPlaybackController : ISpotifyPlaybackController
    {
        
        public SpotifyPlaybackController()
        {
            
        }
        
        public Task PlaySpotifyTrackForUsers(string[] userIds)
        {
            throw new System.NotImplementedException();
        }

        public Task PauseSpotifyPlayerUser()
        {
            throw new System.NotImplementedException();
        }
    }
}