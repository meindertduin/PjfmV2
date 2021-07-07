using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Pjfm.Api.Pages.Gebruiker
{
    public class Login : PageModel
    {
        [BindProperty] public LoginForm Form { get; set; }

        public void OnGet(string returnUrl)
        {
            Form = new LoginForm {ReturnUrl = returnUrl};
        }

        public async Task<IActionResult> OnPost([FromServices] IConfiguration configuration,
            [FromServices] SignInManager<IdentityUser> signInManager)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var signInResult = await signInManager.PasswordSignInAsync(Form.EmailAddress, Form.Password, true, false);
            if (signInResult.Succeeded)
            {
                return Redirect(Form.ReturnUrl);
            }

            return Redirect(configuration.GetValue<string>("ClientUrl"));
        }
    }

    public class LoginForm
    {
        public string ReturnUrl { get; set; }

        [Required(ErrorMessage = "Verplicht veld")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Voer een geldig email in")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Verplicht veld")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}