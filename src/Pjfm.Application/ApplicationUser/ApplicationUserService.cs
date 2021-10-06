using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pjfm.Infrastructure;

namespace Pjfm.Application.ApplicationUser
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly PjfmContext _pjfmContext;

        public ApplicationUserService(PjfmContext pjfmContext)
        {
            _pjfmContext = pjfmContext;
        }
        
        public Task<List<GetApplicationUserResult>> GetApplicationUsers(
            GetUsersRequest request)
        {
            var query = _pjfmContext.Users.AsQueryable();

            if (request.Ids != null)
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }
            if (request.SpotifyAuthenticated.HasValue)
            {
                query = query.Where(x => x.SpotifyAuthenticated == request.SpotifyAuthenticated);
            }
            if (request.SinceLastLoginDate.HasValue)
            {
                query = query.Where(x => x.LastLoginDate > request.SinceLastLoginDate);
            }

            return query.Select(x => new GetApplicationUserResult()
            {
                SpotifyAuthenticated = x.SpotifyAuthenticated,
                UserName = x.UserName,
                Id = x.Id,
            }).AsNoTracking().ToListAsync();
        }
    }

    public interface IApplicationUserService
    {
        public Task<List<GetApplicationUserResult>> GetApplicationUsers(
            GetUsersRequest request);
    }
}