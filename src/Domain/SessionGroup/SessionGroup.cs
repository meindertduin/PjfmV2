
using System.Collections.Generic;

namespace Domain.SessionGroup
{
    public class SessionGroup
    {
        public string Id { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public ICollection<ApplicationUser.ApplicationUser> FillerQueueParticipants { get; set; } = null!;
    }
}