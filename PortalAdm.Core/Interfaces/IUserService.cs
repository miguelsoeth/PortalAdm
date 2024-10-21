using PortalAdm.Core.Entities;
using PortalAdm.Core.DTOs;

namespace PortalAdm.Core.Interfaces;

public interface IUserService
{
    Task<User> RegisterUserAsync(RegistrarRequest request);
    Task<User?> LoginUserAsync(LoginRequest request);
}