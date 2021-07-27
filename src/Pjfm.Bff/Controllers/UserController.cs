using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Pjfm.Bff.Controllers
{
    [Route("gebruiker")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("login")]
        public IActionResult Login()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var props = new AuthenticationProperties
            {
                RedirectUri = _configuration.GetValue<string>("FrontendUrl"),
            };
            return Challenge(props, "cookies", "oidc");
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            return SignOut("cookies", "oidc");
        }
    }
}