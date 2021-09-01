using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/spotify/nummers")]
    public class SpotifyNummersController : PjfmController
    {
        public SpotifyNummersController(IPjfmControllerContext pjfmContext) : base(pjfmContext)
        {
        }

        [HttpGet("update")]
        public IActionResult UpdateGebruikerSpotifyNummers()
        {
            return Ok();
        }
    }
}