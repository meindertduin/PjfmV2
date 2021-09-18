using System.Threading.Tasks;
using Domain.ApplicationUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pjfm.Api.Authentication
{
    public class PjfmSignInManager : SignInManager<ApplicationUser>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public PjfmSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes,
            IUserConfirmation<ApplicationUser> confirmation, IApplicationUserRepository applicationUserRepository) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
            if (result.Succeeded)
            {
                await _applicationUserRepository.SetUserLastLoginDate(userName);
            }

            return result;
        }
    }
}