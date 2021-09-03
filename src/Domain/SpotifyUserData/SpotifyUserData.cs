namespace Domain.SpotifyUserData
{
    public class SpotifyUserData
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}