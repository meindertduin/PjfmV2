using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Api.Models.Gebruiker;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : PjfmController
    {
        public UserController(IPjfmControllerContext pjfmContext) : base(pjfmContext)
        {
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(GetCurrentUserResponseModel), StatusCodes.Status200OK)]
        public IActionResult GetCurrentUserAsync()
        {
            var responseModel = new GetCurrentUserResponseModel()
            {
                UserId = PjfmPrincipal.Id,
                Roles = PjfmPrincipal.Roles,
                UserName = PjfmPrincipal.UserName,
            };

            return Ok(responseModel);
        }
    }
}