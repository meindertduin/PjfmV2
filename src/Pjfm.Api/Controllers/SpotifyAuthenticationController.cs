using System.Text;
using System.Threading.Tasks;
using Domain.SpotifyUserData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Controllers.Base;
using Pjfm.Application.Authentication;
using Pjfm.Application.GebruikerNummer;
using Pjfm.Application.Spotify;
using Pjfm.Common.Extensions;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/spotify/authenticate")]
    public class SpotifyAuthenticationController : PjfmController
    {
        private readonly ISpotifyAuthenticationService _spotifyAuthenticationService;
        private readonly ISpotifyUserDataRepository _spotifyUserDataRepository;
        private readonly IUserTokenService _userTokenService;
        private readonly ISpotifyTrackService _spotifyTrackService;

        public SpotifyAuthenticationController(IPjfmControllerContext pjfmContext,
            ISpotifyAuthenticationService spotifyAuthenticationService,
            ISpotifyUserDataRepository spotifyUserDataRepository,
            IUserTokenService userTokenService,
            ISpotifyTrackService spotifyTrackService) : base(pjfmContext)
        {
            _spotifyAuthenticationService = spotifyAuthenticationService;
            _spotifyUserDataRepository = spotifyUserDataRepository;
            _userTokenService = userTokenService;
            _spotifyTrackService = spotifyTrackService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult Authenticate()
        {
            var authorizationUrl = new StringBuilder("https://accounts.spotify.com/authorize")
                .Append($"?client_id={Configuration.GetValue<string>("Spotify:ClientId")}")
                .Append("&response_type=code")
                .Append($"&state={Helpers.RandomString(30)}")
                .Append($@"&redirect_uri={Configuration.GetValue<string>("Spotify:RedirectUrl")}")
                .Append(
                    "&scope=user-top-read user-read-private streaming user-read-playback-state playlist-read-private playlist-read-collaborative")
                .ToString();

            return Redirect(authorizationUrl);
        }

        [HttpGet("callback")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        public async Task<IActionResult> Callback([FromQuery] string state, [FromQuery] string code)
        {
            // TODO: add validation for the code

            var requestResult = await _spotifyAuthenticationService.RequestAccessToken(code);
            if (requestResult.IsSuccessful)
            {
                await _spotifyUserDataRepository.SetUserRefreshToken(PjfmPrincipal.Id ,requestResult.Result.RefreshToken);
                _userTokenService.StoreUserSpotifyAccessToken(PjfmPrincipal.Id, requestResult.Result.AccessToken, requestResult.Result.ExpiresIn);

                await _spotifyTrackService.UpdateUserSpotifyTracks(PjfmPrincipal.Id);
            }

            return Ok(code);
        }
    }
}