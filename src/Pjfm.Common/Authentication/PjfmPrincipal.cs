using System.Security.Claims;

namespace Pjfm.Common.Authentication
{
    public class PjfmPrincipal : IPjfmPrincipal
    {
        public string? Id { get; }
        private ClaimsPrincipal Principal { get; }
        public PjfmPrincipal(ClaimsPrincipal principal)
        {
            Principal = principal;

            Id = GetGebruikerIdClaimValue(principal);
        }

        private string? GetGebruikerIdClaimValue(ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(PjfmClaimTypes.GebruikerId);

            return idClaim?.Value;
        }

        public bool HasRole(GebruikerRol rol)
        {
            return Principal.IsInRole(rol.ToString());
        }
    }
}