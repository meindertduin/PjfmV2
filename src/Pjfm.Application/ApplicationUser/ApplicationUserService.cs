using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.ApplicationUser;

namespace Pjfm.Application.ApplicationUser
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
        
        public async Task<IEnumerable<Domain.ApplicationUser.ApplicationUser>> GetApplicationUsersSinceLastLogin(
            DateTime date)
        {
            return await _applicationUserRepository.GetApplicationUsersSinceLastLogin(date);
        }
    }

    public interface IApplicationUserService
    {
        public Task<IEnumerable<Domain.ApplicationUser.ApplicationUser>> GetApplicationUsersSinceLastLogin(
            DateTime date);
    }
}