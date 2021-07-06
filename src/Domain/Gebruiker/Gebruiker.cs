using System;
using Pjfm.Common;

namespace Domain.Gebruiker
{
    public class Gebruiker : Entity
    {
        public int Id { get; private set; }
        public Guid? IdentityUserId { get; private set; }
        public string GebruikersNaam { get; set; }

        public Gebruiker(string gebruikersNaam)
        {
            GebruikersNaam = gebruikersNaam;
        }

        public void SetIdentityUserId(Guid userId)
        {
            IdentityUserId = Guard.NotEmpty(userId, nameof(userId));
        }

        public void SetGebruikersNaam(string? gebruikersNaam)
        {
            GebruikersNaam = Guard.NotNullOrEmpty(gebruikersNaam, nameof(gebruikersNaam));
        }
    }
}