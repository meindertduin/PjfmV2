using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/spotify/tracks")]
    public class SpotifyTracksController : PjfmController
    {
        public SpotifyTracksController(IPjfmControllerContext pjfmContext) : base(pjfmContext)
        {
        }

        [HttpGet("update")]
        public IActionResult UpdateUserSpotifyNummers()
        {
            return Ok();
        }
    }
}