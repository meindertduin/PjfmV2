using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Pjfm.Api.Authentication;
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

        public async Task<IActionResult> OnPost([FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] PjfmSignInManager signInManager,
            [FromServices] IConfiguration configuration)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var newUser = new ApplicationUser(Form.Username) { Email = Form.Email };
            var userCreateRequest = await userManager.CreateAsync(newUser, Form.Password);

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
                    await signInManager.PasswordSignInAsync(newUser.UserName, Form.Password, false, false);
                    return Redirect("/");
                }
            }

            return Page();
        }
    }

    public class RegisterForm
    {
        public string ReturnUrl { get; set; } = null!;

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(50, ErrorMessage = "Username is to long")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please fill in a valid Email address")]
        [MaxLength(200, ErrorMessage = "Email address is to long")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Field is required")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Passwords don't match")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Field is required")]
        public string ConfirmPassword { get; set; } = null!;
    }
}