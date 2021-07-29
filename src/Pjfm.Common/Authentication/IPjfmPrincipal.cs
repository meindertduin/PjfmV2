namespace Pjfm.Common.Authentication
{
    public interface IPjfmPrincipal
    {
        int? Id { get; }
        bool HasRole(GebruikerRol rol);
    }
}