using Pjfm.Common.Authentication;

namespace Pjfm.Api.Controllers.Base
{
    public class PjfmControllerContext : IPjfmControllerContext
    {
        public IPjfmPrincipal PjfmPrincipal { get; }

        public PjfmControllerContext(IPjfmPrincipal pjfmPrincipal)
        {
            PjfmPrincipal = pjfmPrincipal;
        }
    }
}