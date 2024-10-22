using PortalAdm.Core.Enums;

namespace PortalAdm.Core.DTOs;

public class RegistrarUsuarioRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    
    public string ClientID { get; set; }
}