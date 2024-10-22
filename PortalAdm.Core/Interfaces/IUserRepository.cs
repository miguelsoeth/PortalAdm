using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    
    Task<User?> GetByCredentialsAsync(String email, String password);
    Task<IEnumerable<UserListResponse>> GetAllAsync();
    Task<string> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
}