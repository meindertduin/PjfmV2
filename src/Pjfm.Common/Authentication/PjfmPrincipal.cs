using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pjfm.Common.Authentication
{
    public class PjfmPrincipal : IPjfmPrincipal
    {
        public string? Id { get; }
        public IEnumerable<UserRole> Roles { get; }
        public string? UserName { get; }

        public PjfmPrincipal(ClaimsPrincipal principal)
        {
            Id = GetUserIdClaimValue(principal);
            Roles = GetUserRollenValue(principal);
            UserName = GetUserUserNameValue(principal);
        }

        private string? GetUserUserNameValue(ClaimsPrincipal principal)
        {
            return principal.FindFirst(PjfmClaimTypes.Name)?.Value;
        }

        private string? GetUserIdClaimValue(ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(PjfmClaimTypes.UserId);

            return idClaim?.Value;
        }

        private IEnumerable<UserRole> GetUserRollenValue(ClaimsPrincipal principal)
        {
            var userRoles = new List<UserRole>();
            foreach (var rol in principal.FindAll(PjfmClaimTypes.Role))
            {
                var parseSucceeded = Enum.TryParse(rol.Value, out UserRole castedRol);
                if (parseSucceeded)
                {
                    if (Enum.GetValues<UserRole>().Contains(castedRol))
                    {
                        userRoles.Add(castedRol);
                    }
                }
            }

            return userRoles;
        }
        
        public bool IsAuthenticated()
        {
            return Roles.Contains(UserRole.User);
        }
    }
}