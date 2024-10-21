using PortalAdm.Core.Enums;

namespace PortalAdm.Core.DTOs;

public class RegistrarRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Roles Role { get; set; }
}