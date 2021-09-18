using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Authentication;
using Pjfm.Api.Controllers.Base;

namespace Pjfm.Api.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : PjfmController
    {
        private readonly PjfmSignInManager _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IPjfmControllerContext pjfmContext, PjfmSignInManager signInManager, IConfiguration configuration) : base(pjfmContext)
        {
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<IActionResult> Logout()
        {
            // TODO: get redirectUrl from logoutId
            
            await _signInManager.SignOutAsync();
            var redirectUrl = _configuration.GetValue<string>("ClientUrl");

            return Redirect(redirectUrl);
        }
    }
}