using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pjfm.Common.Authentication
{
    public class PjfmPrincipal : IPjfmPrincipal
    {
        public string? Id { get; }
        public IEnumerable<GebruikerRol> Rollen { get; }
        public string? GebruikersNaam { get; }

        public PjfmPrincipal(ClaimsPrincipal principal)
        {
            Id = GetGebruikerIdClaimValue(principal);
            Rollen = GetGebruikerRollenValue(principal);
            GebruikersNaam = GetGebruikerGebruikersNaamValue(principal);
        }

        private string? GetGebruikerGebruikersNaamValue(ClaimsPrincipal principal)
        {
            return principal.FindFirst(PjfmClaimTypes.Name)?.Value;
        }

        private string? GetGebruikerIdClaimValue(ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(PjfmClaimTypes.GebruikerId);

            return idClaim?.Value;
        }

        private IEnumerable<GebruikerRol> GetGebruikerRollenValue(ClaimsPrincipal principal)
        {
            var gebruikerRollen = new List<GebruikerRol>();
            foreach (var rol in principal.FindAll(PjfmClaimTypes.Rol))
            {
                var parseSucceeded = Enum.TryParse(rol.Value, out GebruikerRol castedRol);
                if (parseSucceeded)
                {
                    if (Enum.GetValues<GebruikerRol>().Contains(castedRol))
                    {
                        gebruikerRollen.Add(castedRol);
                    }
                }
            }

            return gebruikerRollen;
        }
        
        public bool IsAuthenticated()
        {
            return Rollen.Contains(GebruikerRol.Gebruiker);
        }
    }
}