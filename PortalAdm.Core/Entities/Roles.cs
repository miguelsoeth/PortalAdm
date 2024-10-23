namespace PortalAdm.Core.Entities;

public class Roles
{
    public const string Administrador = "Administrador";
    public const string UsuarioGestor = "Usuário Gestor";
    public const string Usuario = "Usuário";
    
    public static string[] GetAllRoles()
    {
        return new[] { Administrador, UsuarioGestor, Usuario };
    }
    
    public static string[] GetPublicRoles()
    {
        return new[] { UsuarioGestor, Usuario };
    }
}