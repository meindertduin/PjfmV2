using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Api.Models.Gebruiker;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/gebruikers")]
    public class GebruikerController : PjfmController
    {
        public GebruikerController(IPjfmControllerContext pjfmContext) : base(pjfmContext)
        {
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUserAsync()
        {
            var responseModel = new GetCurrentGebruikerResponseModel()
            {
                GebruikersId = PjfmPrincipal!.Id,
                Rollen = new List<GebruikerRolModel>
                {
                    GebruikerRolModel.Gebruiker,
                }
            };

            return Ok(responseModel);
        }
    }
}