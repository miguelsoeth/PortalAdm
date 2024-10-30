using System.Net;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;
using PortalAdm.Infrastructure.Data;
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

    public async Task<User> GetByCredentialsAsync(string email, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null)
            throw new DefaultException("Usuário não encontrado!", HttpStatusCode.BadRequest);

        if (!_passwordHasher.VerifyPassword(user.PasswordHash, password))
            throw new DefaultException("Senha incorreta!", HttpStatusCode.BadRequest);

        return user;
    }
    
    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
            throw new DefaultException("Usuário não encontrado!", HttpStatusCode.BadRequest);
        
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        
        if (existingUser != null)
            throw new DefaultException("Um usuário com esse email já existe!", HttpStatusCode.BadRequest);
        
        await _context.Users.AddAsync(user);
        int result = await _context.SaveChangesAsync();
        
        if (result <= 0)
            throw new DefaultException("Não foi possível adicionar o usuário!", HttpStatusCode.InternalServerError);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetForClientAsync(string userClient)
    {
        Guid userClientId = new Guid(userClient);
        
        return await _context.Users
            .Where(user => user.ClientId == userClientId).ToListAsync();
    }
}