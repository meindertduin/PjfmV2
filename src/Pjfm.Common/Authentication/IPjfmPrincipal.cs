using System.Collections.Generic;

namespace Pjfm.Common.Authentication
{
    public interface IPjfmPrincipal
    {
        string Id { get; }
        public IEnumerable<UserRole> Roles { get; }
        string UserName { get; }
        bool IsAuthenticated();

    }
}