using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Pjfm.Api.Pages.Gebruiker
{
    public class Registreer : PageModel
    {
        [BindProperty] public RegisterForm Form { get; set; }

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

            var newUser = new IdentityUser(Form.Email) {Email = Form.Email};
            var userCreateRequest = await userManager.CreateAsync(newUser, Form.Password);

            if (userCreateRequest.Succeeded)
            {
                var signInResult = await signInManager.PasswordSignInAsync(Form.Email, Form.Password, true, true);
                return Page();
            }

            return Page();
        }
    }

    public class RegisterForm
    {
        public string ReturnUrl { get; set; }

        [Required(ErrorMessage = "veld is verplicht")]
        [MaxLength(50, ErrorMessage = "gebruikersnaam te lang")]
        public string Username { get; set; }

        [Required(ErrorMessage = "veld is verplicht")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Voer een geldig email address in")]
        [MaxLength(200, ErrorMessage = "e-mailaddress te lang")]
        public string Email { get; set; }

        [Required(ErrorMessage = "veld is verplicht")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Wachtwoorden zijn niet hetzelfde")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "veld is verplicht")]
        public string ConfirmPassword { get; set; }
    }
}