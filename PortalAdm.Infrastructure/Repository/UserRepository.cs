using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Interfaces;
using PortalAdm.Infrastructure.Data;
using PortalAdm.SharedKernel.Security;
using PortalAdm.SharedKernel.Util;

namespace PortalAdm.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserRepository(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByCredentialsAsync(string email, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return null;
        }

        return _passwordHasher.VerifyPassword(user.PasswordHash, password) ? user : null;
    }

    public async Task<IEnumerable<UserListResponse>> GetAllAsync()
    {
        return await _context.Users
            .Select(user => new UserListResponse
            {
                Id = user.Id,
                ClientId = user.ClientId,
                Name = user.Name,
                Email = user.Email,
                Role = EnumUtil.GetEnumDescription(user.Role),
                IsActive = user.IsActive
            })
            .ToListAsync();
    }

    public async Task<string> AddAsync(User user)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return "Um usuário com esse email já existe!";
            }
            await _context.Users.AddAsync(user);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return string.Empty;
            }

            return "Erro ao registrar usuário!";
        }
        catch (Exception ex)
        {
            return $"Erro ao adicionar usuário: {ex.Message}";
        }
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}