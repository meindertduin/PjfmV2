using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Domain.SessionGroup
{
    public interface ISessionGroupRepository
    {
        /// <summary>
        /// Creates a SessionGroup in the Database.
        /// </summary>
        /// <param name="groupName">The name of the new SessionGroup</param>
        /// <exception cref="DuplicateNameException">
        ///     A SessionGroup with the given groupName already exists.
        /// </exception>
        Task CreateSessionGroup(string groupName);
        IEnumerable<SessionGroup> GetAllSessionGroups();
        Task AddFillerQueueParticipant(string groupId, ApplicationUser.ApplicationUser user);
        Task RemoveFillerQueueParticipant(string groupId, ApplicationUser.ApplicationUser user);
    }
}