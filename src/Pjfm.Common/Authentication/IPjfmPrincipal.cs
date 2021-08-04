using System.Collections.Generic;

namespace Pjfm.Common.Authentication
{
    public interface IPjfmPrincipal
    {
        string Id { get; }
        public IEnumerable<GebruikerRol> Rollen { get; }
        string GebruikersNaam { get; }
        
    }
}