using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Controllers.Base;

namespace Pjfm.Api.Controllers
{
    [Route("authentication")]
    public class AuthenticationController : PjfmController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IPjfmControllerContext pjfmContext, SignInManager<IdentityUser> signInManager, IConfiguration configuration) : base(pjfmContext)
        {
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            // TODO: get redirectUrl from logoutId
            
            await _signInManager.SignOutAsync();
            var redirectUrl = _configuration.GetValue<string>("ClientUrl");

            return Redirect(redirectUrl);
        }
        
    }
}