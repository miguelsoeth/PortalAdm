using PortalAdm.Core.Entities;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Enums;

namespace PortalAdm.Core.Interfaces;

public interface IUserService
{
    Task<User> RegisterUserAsync(RegistrarUsuarioRequest usuarioRequest, string? userRole);
    Task<User?> LoginUserAsync(LoginRequest request);
    Task<IEnumerable<UserListResponse>> GetAllUsersAsync(string? userRole, string? userClient);
}