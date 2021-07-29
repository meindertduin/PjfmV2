using Microsoft.AspNetCore.Mvc;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Controllers.Base
{
    [ApiController]
    public class PjfmController : ControllerBase
    {
        protected IPjfmControllerContext PjfmContext { get; }
        
        protected IPjfmPrincipal? PjfmPrincipal => PjfmContext.PjfmPrincipal;

        public PjfmController(IPjfmControllerContext pjfmContext)
        {
            PjfmContext = pjfmContext;
        }
    }
}