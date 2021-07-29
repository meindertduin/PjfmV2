using System.Security.Claims;

namespace Pjfm.Common.Authentication
{
    public class PjfmPrincipal : IPjfmPrincipal
    {
        public int? Id { get; }
        public ClaimsPrincipal Principal { get; }
        public PjfmPrincipal(ClaimsPrincipal principal)
        {
            Principal = principal;

            Id = GetGebruikerIdClaimValue(principal);
        }

        private int? GetGebruikerIdClaimValue(ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(PjfmClaimTypes.Rol);
            if (idClaim != null)
            {
                return int.Parse(idClaim.Value);
            }

            return null;
        }

        public bool HasRole(GebruikerRol rol)
        {
            return Principal.IsInRole(rol.ToString());
        }
    }
}