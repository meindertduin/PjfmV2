using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Pjfm.Bff.Controllers
{
    [Route("gebruiker")]
    public class GebruikerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GebruikerController(IConfiguration configuration)
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
        public async Task<IActionResult> Logout([FromQuery] string redirectUrl)
        {
            await Request.HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}