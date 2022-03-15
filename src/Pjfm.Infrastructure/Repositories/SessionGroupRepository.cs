using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using Domain.SessionGroup;
using Microsoft.EntityFrameworkCore;
using Pjfm.Common;

namespace Pjfm.Infrastructure.Repositories
{
    public class SessionGroupRepository : ISessionGroupRepository
    {
        private readonly PjfmContext _pjfmContext;

        public SessionGroupRepository(PjfmContext pjfmContext)
        {
            _pjfmContext = pjfmContext;
        }

        public Task CreateSessionGroup(string groupName)
        {
            var group = _pjfmContext.SessionGroups.FirstOrDefault(s => s.GroupName == groupName);

            if (group == null)
            {
                throw new DuplicateNameException($"A SessionGroup with name: {groupName} already exists.");
            }

            _pjfmContext.SessionGroups.Add(new SessionGroup()
            {
                GroupName = groupName,
                Id = Guid.NewGuid().ToString(),
                FillerQueueParticipants = new List<ApplicationUser>(),
            });

            return _pjfmContext.SaveChangesAsync();
        }

        public IEnumerable<SessionGroup> GetAllSessionGroups()
        {
            return _pjfmContext.SessionGroups.AsNoTracking().ToList();
        }
        
        public Task AddFillerQueueParticipant(string groupId, ApplicationUser user)
        {
            var group = _pjfmContext.SessionGroups.AsNoTracking().FirstOrDefault(g => g.Id == groupId);
            Guard.NotNull(group, nameof(group));

            group!.FillerQueueParticipants.Add(user);
            return _pjfmContext.SaveChangesAsync();
        }

        public Task RemoveFillerQueueParticipant(string groupId, ApplicationUser user)
        {
            var group = _pjfmContext.SessionGroups.AsNoTracking().FirstOrDefault(g => g.Id == groupId);
            Guard.NotNull(group, nameof(group));

            group!.FillerQueueParticipants.Remove(user);
            return _pjfmContext.SaveChangesAsync();
        }
    }
}