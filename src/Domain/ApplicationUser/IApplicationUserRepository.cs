using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.ApplicationUser
{
    public interface IApplicationUserRepository
    {
        public Task<int> SetUserLastLoginDate(string userName);
    }
}