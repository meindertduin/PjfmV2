using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.ApplicationUser
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName) : base(userName)
        {
        }

        public virtual DateTime? LastLoginDate { get; set; }
    }
}