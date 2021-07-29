using System.Security.Claims;

namespace Pjfm.Common.Authentication
{
    public static class PrincipalExtensions
    {
        public static IPjfmPrincipal GetPjfmPrincipal(this ClaimsPrincipal principal)
        {
            return new PjfmPrincipal(principal);
        }
    }
}