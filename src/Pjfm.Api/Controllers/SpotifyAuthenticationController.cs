using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Controllers.Base;
using Pjfm.Common.Extensions;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/spotify/authenticate")]
    public class SpotifyAuthenticationController : PjfmController
    {
        public SpotifyAuthenticationController(IPjfmControllerContext pjfmContext) : base(pjfmContext)
        {
        }
        
        [HttpGet]
        public IActionResult Authenticate()
        {
            var authorizationUrl = new StringBuilder("https://accounts.spotify.com/authorize")
                .Append($"?client_id={Configuration.GetValue<string>("Spotify:ClientId")}")
                .Append("&response_type=code")
                .Append($"&state={Helpers.RandomString(30)}")
                .Append($@"&redirect_uri=https://{Request.Host}/api/spotify/authenticate/callback")
                .Append("&scope=user-top-read user-read-private streaming user-read-playback-state playlist-read-private playlist-read-collaborative")
                .ToString();
            
            return Redirect(authorizationUrl);
        }
        
        [HttpGet("callback")]
        public IActionResult Callback([FromQuery] string state, [FromQuery] string code)
        {
            
            return Ok(code);
        }
    }
}