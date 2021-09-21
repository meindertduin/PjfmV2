using System.Security.Claims;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using Domain.SpotifyTrack;
using Domain.SpotifyUserData;
using Microsoft.AspNetCore.Identity;
using Pjfm.Common.Authentication;

namespace Pjfm.Application.Spotify
{
    public class SpotifyAuthenticationRevoker : ISpotifyAuthenticationRevoker
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ISpotifyTrackRepository _spotifyTrackRepository;
        private readonly ISpotifyUserDataRepository _spotifyUserDataRepository;
        private readonly UserManager<Domain.ApplicationUser.ApplicationUser> _userManager;

        public SpotifyAuthenticationRevoker(IApplicationUserRepository applicationUserRepository,
            ISpotifyTrackRepository spotifyTrackRepository, ISpotifyUserDataRepository spotifyUserDataRepository, UserManager<Domain.ApplicationUser.ApplicationUser> userManager)
        {
            _applicationUserRepository = applicationUserRepository;
            _spotifyTrackRepository = spotifyTrackRepository;
            _spotifyUserDataRepository = spotifyUserDataRepository;
            _userManager = userManager;
        }

        public async Task RevokeUserSpotifyAuthentication(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveClaimAsync(user, new Claim(PjfmClaimTypes.Role, UserRole.SpotifyAuth.ToString()));
            
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