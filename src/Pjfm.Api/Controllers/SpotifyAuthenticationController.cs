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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISpotifyTrackService _spotifyTrackService;
        private readonly ISpotifyUserService _spotifyUserService;
        private readonly StateValidator _stateValidator;

        public SpotifyAuthenticationController(IPjfmControllerContext pjfmContext,
            ISpotifyAuthenticationService spotifyAuthenticationService,
            ISpotifyUserDataRepository spotifyUserDataRepository,
            IUserTokenService userTokenService,
            IApplicationUserRepository applicationUserRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ISpotifyTrackService spotifyTrackService,
            ISpotifyUserService spotifyUserService) : base(pjfmContext)
        {
            _spotifyAuthenticationService = spotifyAuthenticationService;
            _spotifyUserDataRepository = spotifyUserDataRepository;
            _userTokenService = userTokenService;
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _spotifyTrackService = spotifyTrackService;
            _spotifyUserService = spotifyUserService;

            _stateValidator = new StateValidator();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult Authenticate()
        {
            var authorizationUrl = GetAuthorizationUrl("Spotify:RedirectUrl");
            
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

                var result = await _userManager.AddClaimsAsync(user, new[]
                {
                    new Claim(PjfmClaimTypes.Role, UserRole.SpotifyAuth.ToString()),
                });

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    
                    _userTokenService.StoreUserSpotifyAccessToken(PjfmPrincipal.Id, requestResult.Result.AccessToken,
                        requestResult.Result.ExpiresIn);
                    await _spotifyTrackService.SetUserSpotifyTracks(PjfmPrincipal.Id);
                    
                    return Redirect("/");
                }
            }
            
            return BadRequest();
        }
        
        [HttpGet("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult Login()
        {
            var authorizationUrl = GetAuthorizationUrl("Spotify:LoginRedirectUrl");

            return Redirect(authorizationUrl);
        }
        
        [HttpGet("logincallback")]
        [AllowAnonymous]
        public async Task<IActionResult> SpotifyLoginCallback(string code)
        {
            var requestResult = await _spotifyAuthenticationService.RequestRegisterAccessToken(code);
            if (requestResult.IsSuccessful)
            {
                var userData = await _spotifyUserService.GetSpotifyUserData(requestResult.Result.AccessToken);

                if (userData != null)
                {
                    var user = await _userManager.FindByEmailAsync(userData.Email);
                    if (user == null)
                    {
                        return Redirect($"/User/SpotifyRegisterCallback?accessToken={requestResult.Result.AccessToken}");
                    }

                    await _signInManager.SignInAsync(user, false);
                }

                return Redirect("/");
            }

            return BadRequest("Invalid code");
        }

        private string? GetAuthorizationUrl(string urlKey)
        {
            var state = _stateValidator.GenerateNewState();

            var authorizationUrl = new StringBuilder("https://accounts.spotify.com/authorize")
                .Append($"?client_id={Configuration.GetValue<string>("Spotify:ClientId")}")
                .Append("&response_type=code")
                .Append($"&state={state}")
                .Append($@"&redirect_uri={Configuration.GetValue<string>(urlKey)}")
                .Append(
                    "&scope=user-top-read user-read-private user-read-email streaming user-read-playback-state playlist-read-private playlist-read-collaborative")
                .ToString();
            return authorizationUrl;
        }
    }
}