using System.Collections.Generic;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Models.Gebruiker
{
    public class GetCurrentUserResponseModel
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public IEnumerable<UserRole> Roles { get; set; } = null!;
    }
}