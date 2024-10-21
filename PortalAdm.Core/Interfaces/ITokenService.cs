using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface ITokenService
{
    Task<string> GenerateJwtToken(User user);
}