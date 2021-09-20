
namespace Pjfm.Application.ApplicationUser
{
    public class GetApplicationUserResult
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public bool SpotifyAuthenticated { get; set; }
    }
}