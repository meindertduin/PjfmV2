using System;

namespace Pjfm.Application.ApplicationUser
{
    public class GetUsersRequest
    {
        public DateTime? SinceLastLoginDate { get; set; } = null;
        public string[] Ids { get; set; } = null;
        public bool? SpotifyAuthenticated { get; set; } = null;
    }
}