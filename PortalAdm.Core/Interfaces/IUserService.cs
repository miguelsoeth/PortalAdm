using PortalAdm.Core.Entities;
using PortalAdm.Core.DTOs;

namespace PortalAdm.Core.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(string name, string email, string pwd, string role, string? clientId, string? userRole, string? userClient);
    Task<User> LoginUserAsync(string email, string pwd);
    Task<IEnumerable<User>> GetAllUsersAsync(string userRole, string userClient);
    Task<User> GetUserByEmailAsync(string email);
}