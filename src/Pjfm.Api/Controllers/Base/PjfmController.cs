using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Controllers.Base
{
    [ApiController]
    public class PjfmController : ControllerBase
    {
        protected IPjfmControllerContext PjfmContext { get; }
        
        protected IPjfmPrincipal PjfmPrincipal => PjfmContext.PjfmPrincipal;
        protected IConfiguration Configuration => PjfmContext.Configuration;

        public PjfmController(IPjfmControllerContext pjfmContext)
        {
            PjfmContext = pjfmContext;
        }
    }
}