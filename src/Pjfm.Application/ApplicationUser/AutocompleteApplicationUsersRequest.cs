namespace Pjfm.Application.ApplicationUser
{
    public class AutocompleteApplicationUsersRequest
    {
        public bool SpotifyAuthenticated { get; set; } = false;
        public string Query { get; set; } = null!;
        public int Limit { get; set; }
    }
}