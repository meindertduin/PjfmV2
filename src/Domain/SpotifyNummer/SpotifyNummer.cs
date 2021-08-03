using System;
using System.Collections.Generic;

namespace Domain.SpotifyNummer
{
    public class SpotifyNummer : Entity
    {
        public int Id { get; set; }
        public string Titel { get; set; } = null!;
        public string GebruikerId { get; set; } = null!;
        public string SpotifyNummerId { get; set; } = null!;
        public IEnumerable<string> Artists { get; set; } = null!;
        public TrackTermijn TrackTermijn { get; set; }
        public int NummerDuurMs { get; set; }
        public DateTime AangemaaktOp { get; set; }
    }
}