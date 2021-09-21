using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Authentication;

namespace Pjfm.Api.Pages.Gebruiker
{
    public class Login : PageModel
    {
        [BindProperty] public LoginForm Form { get; set; } = null!;

        public void OnGet(string returnUrl)
        {
            Form = new LoginForm {ReturnUrl = returnUrl};
        }

        public async Task<IActionResult> OnPost([FromServices] IConfiguration configuration,
            [FromServices] PjfmSignInManager signInManager, [FromServices] UserManager<ApplicationUser> userManager)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var user = await userManager.FindByEmailAsync(Form.EmailAddress);
            var signInResult = await signInManager.PasswordSignInAsync(user.UserName, Form.Password, true, false);
            if (signInResult.Succeeded)
            {
                return Redirect(Form.ReturnUrl);
            }

            return Redirect(configuration.GetValue<string>("ClientUrl"));
        }
    }

    public class LoginForm
    {
        public string ReturnUrl { get; set; } = null!;

        [Required(ErrorMessage = "Field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please fill in a valid Email address")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "Field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}