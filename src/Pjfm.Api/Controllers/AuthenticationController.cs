using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Authentication;
using Pjfm.Api.Controllers.Base;

namespace Pjfm.Api.Controllers
{
    [Route("authentication")]
    public class AuthenticationController : PjfmController
    {
        private readonly PjfmSignInManager _signInManager;

        public AuthenticationController(IPjfmControllerContext pjfmContext, PjfmSignInManager signInManager,
            IConfiguration configuration) : base(pjfmContext)
        {
            _signInManager = signInManager;
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }
    }
}