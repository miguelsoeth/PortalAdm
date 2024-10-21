using System.ComponentModel;

namespace PortalAdm.Core.Enums;
public enum Roles
{
    [Description("Usuário")]
    Usuario,

    [Description("Usuário Gestor")]
    UsuarioGestor,

    [Description("Administrador")]
    Administrador
}