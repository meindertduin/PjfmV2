namespace Domain.SpotifyGebruikerData
{
    public class SpotifyGebruikerData
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; } = null!;
        public string GebruikerId { get; set; } = null!;
    }
}