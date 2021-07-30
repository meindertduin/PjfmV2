namespace Pjfm.Common.Authentication
{
    public interface IPjfmPrincipal
    {
        string? Id { get; }
        bool HasRole(GebruikerRol rol);
    }
}