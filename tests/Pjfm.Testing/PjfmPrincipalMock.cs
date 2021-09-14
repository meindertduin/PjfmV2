using System;
using System.Collections.Generic;
using System.Security.Claims;
using Pjfm.Common.Authentication;

namespace Pjfm.Testing
{
    public class PjfmPrincipalMock : PjfmPrincipal
    {
        public PjfmPrincipalMock() : this(Guid.NewGuid().ToString(), "test", new [] {UserRole.User})
        {
            
        }

        public PjfmPrincipalMock(string userId, string userName, IEnumerable<UserRole> userRoles) : base(CreatePrincipal(userId, userName, userRoles))
        {
            
        }

        private static ClaimsPrincipal CreatePrincipal(string userId, string userName ,IEnumerable<UserRole> userRoles)
        {
            var claims = new List<Claim>()
            {
                new(PjfmClaimTypes.UserId, userId),
                new(PjfmClaimTypes.Name, userName),
            };

            foreach (var userRole in userRoles)
            {
                claims.Add(new (PjfmClaimTypes.Role, userRole.ToString()));
            }

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            return principal;
        }
    }
}