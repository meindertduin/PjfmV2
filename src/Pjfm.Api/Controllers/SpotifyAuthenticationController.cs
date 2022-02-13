using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using Domain.SpotifyUserData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Authentication;
using Pjfm.Api.Controllers.Base;
using Pjfm.Application.Authentication;
using Pjfm.Application.GebruikerNummer;
using Pjfm.Application.Spotify;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/spotify/authenticate")]
    public class SpotifyAuthenticationController : PjfmController
    {
        private readonly ISpotifyAuthenticationService _spotifyAuthenticationService;
        private readonly ISpotifyUserDataRepository _spotifyUserDataRepository;
        private readonly IUserTokenService _userTokenService;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISpotifyTrackService _spotifyTrackService;
        private readonly StateValidator _stateValidator;

        public SpotifyAuthenticationController(IPjfmControllerContext pjfmContext,
            ISpotifyAuthenticationService spotifyAuthenticationService,
            ISpotifyUserDataRepository spotifyUserDataRepository,
            IUserTokenService userTokenService,
            IApplicationUserRepository applicationUserRepository,
            UserManager<ApplicationUser> userManager,
            ISpotifyTrackService spotifyTrackService) : base(pjfmContext)
        {
            _spotifyAuthenticationService = spotifyAuthenticationService;
            _spotifyUserDataRepository = spotifyUserDataRepository;
            _userTokenService = userTokenService;
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
            _spotifyTrackService = spotifyTrackService;

            _stateValidator = new StateValidator();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult Authenticate()
        {
            var state = _stateValidator.GenerateNewState();

            var authorizationUrl = new StringBuilder("https://accounts.spotify.com/authorize")
                .Append($"?client_id={Configuration.GetValue<string>("Spotify:ClientId")}")
                .Append("&response_type=code")
                .Append($"&state={state}")
                .Append($@"&redirect_uri={Configuration.GetValue<string>("Spotify:RedirectUrl")}")
                .Append(
                    "&scope=user-top-read user-read-private streaming user-read-playback-state playlist-read-private playlist-read-collaborative")
                .ToString();

            return Redirect(authorizationUrl);
        }

        [HttpGet("callback")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Callback([FromQuery] string state, [FromQuery] string code)
        {
            if (!_stateValidator.ValidateState(state))
            {
                return BadRequest("Could not validate state.");
            }

            var requestResult = await _spotifyAuthenticationService.RequestAccessToken(code);
            if (requestResult.IsSuccessful)
            {
                await _spotifyUserDataRepository.SetUserRefreshToken(PjfmPrincipal.Id,
                    requestResult.Result.RefreshToken);
                await _applicationUserRepository.SetUserSpotifyAuthenticated(PjfmPrincipal.Id, true);

                var user = await _userManager.GetUserAsync(HttpContext.User);
                
                await _userManager.AddClaimsAsync(user, new[]
                {
                    new Claim(PjfmClaimTypes.Role, UserRole.SpotifyAuth.ToString()),
                });
                
                _userTokenService.StoreUserSpotifyAccessToken(PjfmPrincipal.Id, requestResult.Result.AccessToken,
                    requestResult.Result.ExpiresIn);
                await _spotifyTrackService.SetUserSpotifyTracks(PjfmPrincipal.Id);
            }

            return Redirect("/");
        }
    }
}