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
        private readonly IConfiguration _configuration;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthenticationController(IPjfmControllerContext pjfmContext, PjfmSignInManager signInManager,
            IConfiguration configuration, IIdentityServerInteractionService interactionService) : base(pjfmContext)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _interactionService = interactionService;
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logoutContext = await _interactionService.GetLogoutContextAsync(logoutId);

            await _signInManager.SignOutAsync();

            return Redirect(logoutContext.PostLogoutRedirectUri);
        }
    }
}