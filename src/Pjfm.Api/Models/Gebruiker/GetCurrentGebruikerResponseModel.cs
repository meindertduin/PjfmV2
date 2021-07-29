using System.Collections.Generic;

namespace Pjfm.Api.Models.Gebruiker
{
    public class GetCurrentGebruikerResponseModel
    {
        public int? GebruikersId { get; set; }
        public string? GebruikersNaam { get; set; }
        public IEnumerable<GebruikerRolModel> Rollen { get; set; } = null!;
    }

    public enum GebruikerRolModel
    {
        Gebruiker,
        Dj,
    }
}