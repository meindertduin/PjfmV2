using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Domain.ApplicationUser;
using Domain.SpotifyUserData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pjfm.Api.Authentication;
using Pjfm.Application.Spotify;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Pages.User
{
    public class SpotifyRegisterCallback : PageModel
    {
        private readonly ISpotifyAuthenticationService _spotifyAuthenticationService;
        private readonly ISpotifyUserDataRepository _spotifyUserDataRepository;

        [BindProperty] public RegisterForm Form { get; set; } = null!;

        public SpotifyRegisterCallback(ISpotifyAuthenticationService spotifyAuthenticationService, ISpotifyUserDataRepository spotifyUserDataRepository)
        {
            _spotifyAuthenticationService = spotifyAuthenticationService;
            _spotifyUserDataRepository = spotifyUserDataRepository;
        }
        
        public async Task OnGet([FromQuery] string state, [FromQuery] string code)
        {
            // TODO add validator for the state
            var requestResult = await _spotifyAuthenticationService.RequestRegisterAccessToken(code);
            if (requestResult.IsSuccessful)
            {
                Form = new RegisterForm() {SpotifyAccessToken = requestResult.Result.AccessToken};
            }
        }
        
        public async Task<IActionResult> OnPost([FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] PjfmSignInManager signInManager,
            [FromServices] ISpotifyUserService spotifyUserService)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var userData = await spotifyUserService.GetSpotifyUserData(Form.SpotifyAccessToken);
            if (userData != null)
            {
                var newUser = new ApplicationUser(Form.UserName) { Email = userData.Email };
                var userCreateRequest = await userManager.CreateAsync(newUser);
                
                if (userCreateRequest.Succeeded)
                {
                    var claimsAddResult = await userManager.AddClaimsAsync(newUser, new[]
                    {
                        new Claim(PjfmClaimTypes.Role, UserRole.User.ToString()),
                        new Claim(PjfmClaimTypes.UserId, newUser.Id),
                        new Claim(PjfmClaimTypes.Name, newUser.UserName),
                    });

                    if (claimsAddResult.Succeeded)
                    {
                        await signInManager.SignInAsync(newUser, false);
                        return Redirect("/api/spotify/authenticate");
                    }
                }
            }

            return Page();
        }
        
        public class RegisterForm
        {
            public string UserName { get; set; } = null!;
            public string SpotifyAccessToken { get; set; } = null!;
        }
    }
}