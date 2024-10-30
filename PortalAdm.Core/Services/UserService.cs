using System.Net;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;
using PortalAdm.SharedKernel;
using PortalAdm.SharedKernel.Util;

namespace PortalAdm.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterUserAsync(string name, string email, string pwd, string role, string? clientId, string? userRole, string? userClient)
    {
        if (userRole == null || userClient == null)
            throw new DefaultException("Erro ao adquirir informações do usuário atual!", HttpStatusCode.BadRequest);

        if (userRole == Roles.UsuarioGestor && role == Roles.Administrador)
            throw new DefaultException("Não é permitido adicionar um Administrador!", HttpStatusCode.Forbidden);

        if (userRole == Roles.UsuarioGestor)
            clientId = userClient;

        if (!Roles.GetAllRoles().Contains(role))
            throw new DefaultException("Papel inexistente!", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(name))
            throw new DefaultException("Nome é obrigatório", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(email))
            throw new DefaultException("Email é obrigatório", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(pwd))
            throw new DefaultException("Senha é obrigatória", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(role))
            throw new DefaultException("Papel é obrigatório", HttpStatusCode.BadRequest);

        if (userRole == Roles.Administrador && string.IsNullOrWhiteSpace(clientId))
            throw new DefaultException("ID do Cliente é obrigatório", HttpStatusCode.BadRequest);

        var passwordHash = _passwordHasher.HashPassword(pwd);

        var user = new User(name, email, passwordHash, role, new Guid(clientId!));

        await _userRepository.AddAsync(user);
    }
    
    public async Task<User> LoginUserAsync(string email, string pwd)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pwd))
            throw new DefaultException("Email e/ou password não podem estar vazios!", HttpStatusCode.BadRequest);

        var user = await _userRepository.GetByCredentialsAsync(email, pwd);
        return user;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(string userRole, string userClient)
    {
        if (userRole.Equals(Roles.UsuarioGestor))
        {
            return await _userRepository.GetForClientAsync(userClient);
        }
        return await _userRepository.GetAllAsync();
    }

    public async Task<User> GetUserByEmailAsync(string? email)
    {
        if (email == null)
            throw new DefaultException("Erro ao adquirir informações do usuário!", HttpStatusCode.BadRequest);
        
        return await _userRepository.GetByEmailAsync(email);
    }
}