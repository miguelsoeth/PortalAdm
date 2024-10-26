using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
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

    public async Task<AuthResponse> RegisterUserAsync(RegistrarUsuarioRequest usuarioRequest, string? userRole, string? userClient)
    {
        if (userRole == null || userClient == null)
            return new AuthResponse(false, String.Empty, "Erro ao adquirir informações do usuário atual!");
        
        if (userRole == Roles.UsuarioGestor && usuarioRequest.Role == Roles.Administrador)
            return new AuthResponse(false, "Forbid", "Não é permitido adicionar um Administrador!");
        
        if (userRole == Roles.UsuarioGestor)
            usuarioRequest.ClientId = userClient;
        
        if (!Roles.GetAllRoles().Contains(usuarioRequest.Role))
            return new AuthResponse(false, String.Empty, "Papel inexistente!");
        
        if (string.IsNullOrWhiteSpace(usuarioRequest.Name))
        {
            return new AuthResponse(false, String.Empty, "Nome é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(usuarioRequest.Email))
        {
            return new AuthResponse(false, String.Empty, "Email é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(usuarioRequest.Password))
        {
            return new AuthResponse(false, String.Empty, "Senha é obrigatória");
        }

        if (string.IsNullOrWhiteSpace(usuarioRequest.Role))
        {
            return new AuthResponse(false, String.Empty, "Papel é obrigatório");
        }
        
        if (userRole == Roles.Administrador && string.IsNullOrWhiteSpace(usuarioRequest.ClientId))
        {
            return new AuthResponse(false, String.Empty, "ClientID é obrigatório");
        }

        var passwordHash = _passwordHasher.HashPassword(usuarioRequest.Password);

        var user = new User(usuarioRequest.Name, usuarioRequest.Email, passwordHash, usuarioRequest.Role, new Guid(usuarioRequest.ClientId!));

        string success = await _userRepository.AddAsync(user);

        if (success.Equals(string.Empty))
        {
            return new AuthResponse(true, String.Empty, "Usuário registrado com sucesso!");
        }
        
        return new AuthResponse(false, String.Empty, success);
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

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }
}