using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.SpotifyTrack
{
    public interface ISpotifyTrackRepository
    {
        Task<List<SpotifyTrack>> GetUserSpotifyTracksByUserId(string userId);
        Task SetUserSpotifyTracks(IEnumerable<SpotifyTrack> spotifyTracks, string userId);
        Task RemoveUserSpotifyTracks(string userId);

        Task<List<SpotifyTrack>> GetRandomUserSpotifyTracks(string[] userIds,
            IEnumerable<TrackTerm> terms, int amount);
    }
}