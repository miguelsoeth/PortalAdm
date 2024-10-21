using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Interfaces;
using PortalAdm.SharedKernel.Security;

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

    public async Task<User> RegisterUserAsync(RegistrarRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || 
            string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Name, email, and password cannot be empty.");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User(request.Name, request.Email, passwordHash, request.Role);

        await _userRepository.AddAsync(user);

        return user;
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
}