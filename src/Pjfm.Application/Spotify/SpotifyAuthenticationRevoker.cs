using System.Threading.Tasks;
using Domain.ApplicationUser;
using Domain.SpotifyTrack;
using Domain.SpotifyUserData;

namespace Pjfm.Application.Spotify
{
    public class SpotifyAuthenticationRevoker : ISpotifyAuthenticationRevoker
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ISpotifyTrackRepository _spotifyTrackRepository;
        private readonly ISpotifyUserDataRepository _spotifyUserDataRepository;

        public SpotifyAuthenticationRevoker(IApplicationUserRepository applicationUserRepository,
            ISpotifyTrackRepository spotifyTrackRepository, ISpotifyUserDataRepository spotifyUserDataRepository)
        {
            _applicationUserRepository = applicationUserRepository;
            _spotifyTrackRepository = spotifyTrackRepository;
            _spotifyUserDataRepository = spotifyUserDataRepository;
        }

        public async Task RevokeUserSpotifyAuthentication(string userId)
        {
            await _applicationUserRepository.SetUserSpotifyAuthenticated(userId, false);
            await _spotifyUserDataRepository.RemoveUserSpotifyData(userId);
            await _spotifyTrackRepository.RemoveUserSpotifyTracks(userId);
        }
    }

    public interface ISpotifyAuthenticationRevoker
    {
        Task RevokeUserSpotifyAuthentication(string userId);
    }
}