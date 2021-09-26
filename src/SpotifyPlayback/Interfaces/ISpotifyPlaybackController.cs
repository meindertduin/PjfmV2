using System.Threading.Tasks;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Interfaces
{
    public interface ISpotifyPlaybackController
    {
        Task PlaySpotifyTrackForUsers(PlaybackScheduledTrack spotifyScheduledTrack);
    }
}