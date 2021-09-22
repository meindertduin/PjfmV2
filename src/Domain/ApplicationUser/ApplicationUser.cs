using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.ApplicationUser
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName) : base(userName)
        {
        }

        public virtual DateTime? LastLoginDate { get; set; }
        public ICollection<SpotifyTrack.SpotifyTrack> SpotifyTracks { get; set; } = null!;
        public int SpotifyUserDataId { get; set; }
        public SpotifyUserData.SpotifyUserData SpotifyUserData { get; set; } = null!;
        public bool SpotifyAuthenticated { get; set; }
    }
}