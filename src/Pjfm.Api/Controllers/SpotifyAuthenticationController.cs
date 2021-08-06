using System.Text;
using System.Threading.Tasks;
using Domain.SpotifyGebruikerData;
using Domain.SpotifyNummer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Authentication;
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
        private readonly ISpotifyGebruikersDataRepository _spotifyGebruikersDataRepository;
        private readonly IGebruikerTokenService _gebruikerTokenService;
        private readonly ISpotifyNummerService _spotifyNummerService;

        public SpotifyAuthenticationController(IPjfmControllerContext pjfmContext,
            ISpotifyAuthenticationService spotifyAuthenticationService,
            ISpotifyGebruikersDataRepository spotifyGebruikersDataRepository,
            IGebruikerTokenService gebruikerTokenService,
            ISpotifyNummerService spotifyNummerService) : base(pjfmContext)
        {
            _spotifyAuthenticationService = spotifyAuthenticationService;
            _spotifyGebruikersDataRepository = spotifyGebruikersDataRepository;
            _gebruikerTokenService = gebruikerTokenService;
            _spotifyNummerService = spotifyNummerService;
        }

        [HttpGet]
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
        public async Task<IActionResult> Callback([FromQuery] string state, [FromQuery] string code)
        {
            // TODO: add validation for the code

            var requestResult = await _spotifyAuthenticationService.RequestAccessToken(code);
            if (requestResult.IsSuccessful)
            {
                await _spotifyGebruikersDataRepository.SetGebruikerRefreshToken(PjfmPrincipal.Id ,requestResult.Result.RefreshToken);
                _gebruikerTokenService.StoreGebruikerSpotifyAccessToken(PjfmPrincipal.Id, requestResult.Result.AccessToken, requestResult.Result.ExpiresIn);

                await _spotifyNummerService.UpdateGebruikerSpotifyNummers(PjfmPrincipal.Id);
            }

            return Ok(code);
        }
    }
}