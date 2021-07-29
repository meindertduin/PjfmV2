using System.Security.Claims;

namespace Pjfm.Common.Authentication
{
    public class PjfmClaimTypes
    {
        public const string GebruikerId = "pjfm_gebruiker_id";
        public const string Rol = ClaimTypes.Role;
        public const string Emailaddres = ClaimTypes.Upn;
    }
}