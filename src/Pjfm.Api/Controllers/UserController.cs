using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Api.Models.Gebruiker;
using Pjfm.Application.ApplicationUser;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : PjfmController
    {
        private readonly IApplicationUserService _applicationUserService;

        public UserController(IPjfmControllerContext pjfmContext, IApplicationUserService applicationUserService) : base(pjfmContext)
        {
            _applicationUserService = applicationUserService;
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(GetCurrentUserResponseModel), StatusCodes.Status200OK)]
        public IActionResult GetCurrentUserAsync()
        {
            var responseModel = new GetCurrentUserResponseModel()
            {
                UserId = PjfmPrincipal.Id,
                Roles = PjfmPrincipal.Roles,
                UserName = PjfmPrincipal.UserName,
            };

            return Ok(responseModel);
        }

        [HttpGet("autocomplete")]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Autocomplete(string query, int limit = 20) 
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest();
            }

            var users = await _applicationUserService.AutocompleteApplicationUsers(new AutocompleteApplicationUsersRequest()
            {
                Query = query,
                SpotifyAuthenticated = true,
                Limit = limit,
            });

            return Ok(users);
        }
    }
}