using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Pages.User
{
    public class Register : PageModel
    {
        [BindProperty] public RegisterForm Form { get; set; } = null!;

        public void OnGet([FromServices] IConfiguration configuration)
        {
            Form = new RegisterForm() {ReturnUrl = configuration.GetValue<string>("ClientUrl")};
        }

        public async Task<IActionResult> OnPost([FromServices] UserManager<IdentityUser> userManager,
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] IConfiguration configuration)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var newUser = new IdentityUser(Form.Username) { Email = Form.Email };
            var userCreateRequest = await userManager.CreateAsync(newUser, Form.Password);

            if (userCreateRequest.Succeeded)
            {
                var claimsAddResult = await userManager.AddClaimsAsync(newUser, new[]
                {
                    new Claim(PjfmClaimTypes.Rol, UserRole.User.ToString()),
                    new Claim(PjfmClaimTypes.UserId, newUser.Id),
                });

                if (claimsAddResult.Succeeded)
                {
                    var redirectUrl = configuration.GetValue<string>("ClientUrl"); 
                    return Redirect(redirectUrl);
                }
            }

            return Page();
        }
    }

    public class RegisterForm
    {
        public string ReturnUrl { get; set; } = null!;

        [Required(ErrorMessage = "veld is verplicht")]
        [MaxLength(50, ErrorMessage = "gebruikersnaam te lang")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "veld is verplicht")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Voer een geldig email address in")]
        [MaxLength(200, ErrorMessage = "e-mailaddress te lang")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "veld is verplicht")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Wachtwoorden zijn niet hetzelfde")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "veld is verplicht")]
        public string ConfirmPassword { get; set; } = null!;
    }
}