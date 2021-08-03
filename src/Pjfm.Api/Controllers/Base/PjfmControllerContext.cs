using Microsoft.Extensions.Configuration;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Controllers.Base
{
    public class PjfmControllerContext : IPjfmControllerContext
    {
        public IPjfmPrincipal PjfmPrincipal { get; }
        public IConfiguration Configuration { get; }

        public PjfmControllerContext(IPjfmPrincipal pjfmPrincipal, IConfiguration configuration)
        {
            PjfmPrincipal = pjfmPrincipal;
            Configuration = configuration;
        }
    }
}