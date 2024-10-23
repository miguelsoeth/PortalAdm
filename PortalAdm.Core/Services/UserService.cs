using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Interfaces;
using PortalAdm.SharedKernel;
using PortalAdm.SharedKernel.Security;
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

    public async Task<User> RegisterUserAsync(RegistrarUsuarioRequest usuarioRequest, string userRole, string userClient)
    {
        if (userRole == Roles.UsuarioGestor && usuarioRequest.Role == Roles.Administrador)
        {
            return new User("Não é possível registrar um Administrador!");
        }
        
        if (string.IsNullOrWhiteSpace(usuarioRequest.Name))
        {
            return new User("Nome é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(usuarioRequest.Email))
        {
            return new User("Email é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(usuarioRequest.Password))
        {
            return new User("Senha é obrigatória");
        }

        if (string.IsNullOrWhiteSpace(usuarioRequest.Role))
        {
            return new User("Papel é obrigatório");
        }
        
        if (userRole == Roles.Administrador && string.IsNullOrWhiteSpace(usuarioRequest.ClientId))
        {
            return new User("ClientID é obrigatório");
        }

        if (userRole == Roles.UsuarioGestor)
        {
            usuarioRequest.ClientId = userClient;
        }

        var passwordHash = _passwordHasher.HashPassword(usuarioRequest.Password);

        var user = new User(usuarioRequest.Name, usuarioRequest.Email, passwordHash, usuarioRequest.Role, new Guid(usuarioRequest.ClientId));

        string success = await _userRepository.AddAsync(user);

        if (success.Equals(string.Empty))
        {
            return user;
        }
        
        return new User(success);
    }
    
    public async Task<User?> LoginUserAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Email and password cannot be empty.");
        }

        var user = await _userRepository.GetByCredentialsAsync(request.Email, request.Password);

        return user;
    }

    public async Task<IEnumerable<UserListResponse>> GetAllUsersAsync(string userRole, string userClient)
    {
        if (userRole.Equals(Roles.UsuarioGestor))
        {
            return await _userRepository.GetForClientAsync(userClient);
        }
        return await _userRepository.GetAllAsync();
    }
}