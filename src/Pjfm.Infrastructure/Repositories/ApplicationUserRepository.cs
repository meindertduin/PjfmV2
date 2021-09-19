using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using Microsoft.EntityFrameworkCore;

namespace Pjfm.Infrastructure.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly PjfmContext _pjfmContext;

        public ApplicationUserRepository(PjfmContext pjfmContext)
        {
            _pjfmContext = pjfmContext;
        }

        public Task<int> SetUserLastLoginDate(string userName)
        {
            var user = _pjfmContext.Users.FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                user.LastLoginDate = DateTime.Now;
            }

            return _pjfmContext.SaveChangesAsync();
        }

        public Task<List<ApplicationUser>> GetApplicationUsersSinceLastLogin(DateTime date)
        {
            return _pjfmContext.Users.Where(x => x.LastLoginDate > date).ToListAsync();
        }
    }
}