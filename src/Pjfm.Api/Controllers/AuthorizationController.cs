using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Pjfm.Api.Controllers.Base;

namespace Pjfm.Api.Controllers
{
    public class AuthorizationController : PjfmController
    {
        public AuthorizationController(IPjfmControllerContext pjfmContext) : base(pjfmContext)
        {
        }

        [HttpPost("~/connect/token")]
        public IActionResult Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("Openiddict request cannot be retrieved");

            ClaimsPrincipal claimsPrincipal;

            if (request.IsClientCredentialsGrantType())
            {
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                
                identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());
                identity.AddClaim("some-claim", "some-value", OpenIddictConstants.Destinations.AccessToken);

                claimsPrincipal = new ClaimsPrincipal(identity);

                claimsPrincipal.SetScopes(request.GetScopes()); 
            }
            else
            {
                throw new InvalidOperationException("The specified grant type is not supported");
            }

            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            var claims = new List<Claim>
            {
                new Claim(OpenIddictConstants.Claims.Subject, result.Principal.Identity.Name),
                new Claim("some claim", "some value").SetDestinations(OpenIddictConstants.Destinations.AccessToken)
            };

            var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            claimsPrincipal.SetScopes(request.GetScopes());
            
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }        
    }
}