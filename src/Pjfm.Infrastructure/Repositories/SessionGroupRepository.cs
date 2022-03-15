using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<string> CreateSessionGroup(string groupName)
        {
            var group = await _pjfmContext.SessionGroups.FirstOrDefaultAsync(s => s.GroupName == groupName);

            if (group != null)
            {
                throw new DuplicateNameException($"A SessionGroup with name: {groupName} already exists.");
            }

            var id = Guid.NewGuid().ToString();
            _pjfmContext.SessionGroups.Add(new SessionGroup()
            {
                GroupName = groupName,
                Id = id,
                FillerQueueParticipants = new List<ApplicationUser>(),
            });

            await _pjfmContext.SaveChangesAsync();
            return id;
        }

        public Task<List<SessionGroup>> GetAllSessionGroups()
        {
            return _pjfmContext.SessionGroups
                .Include(s => s.FillerQueueParticipants)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<SessionGroup?> FindSessionGroupById(string groupId)
        {
            return _pjfmContext.SessionGroups
                .Include(s => s.FillerQueueParticipants)
                .FirstOrDefaultAsync(s => s.Id == groupId)!;
        }
        
        public async Task SetFillerQueueParticipants(string groupId, List<ApplicationUser> users)
        {
            var group = await _pjfmContext.SessionGroups
                .Include(s => s.FillerQueueParticipants)
                .FirstOrDefaultAsync(g => g.Id == groupId);
            Guard.NotNull(group, nameof(group));

            group!.FillerQueueParticipants = users;
            await _pjfmContext.SaveChangesAsync();
        }
    }
}