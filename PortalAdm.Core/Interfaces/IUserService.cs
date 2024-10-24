using PortalAdm.Core.Entities;
using PortalAdm.Core.DTOs;

namespace PortalAdm.Core.Interfaces;

public interface IUserService
{
    Task<AuthResponse> RegisterUserAsync(RegistrarUsuarioRequest usuarioRequest, string? userRole, string? userClient);
    Task<User?> LoginUserAsync(LoginRequest request);
    Task<IEnumerable<UserListResponse>> GetAllUsersAsync(string userRole, string userClient);
    Task<User?> GetUserByEmailAsync(string email);
}