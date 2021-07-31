using System.Collections.Generic;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Models.Gebruiker
{
    public class GetCurrentGebruikerResponseModel
    {
        public string? GebruikersId { get; set; }
        public string? GebruikersNaam { get; set; }
        public IEnumerable<GebruikerRol>? Rollen { get; set; } = null!;
    }
}