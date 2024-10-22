using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using PortalAdm.Core.Entities;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Enums;
using PortalAdm.Core.Interfaces;
using PortalAdm.SharedKernel.Util;

namespace PortalAdm.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private ITokenService _tokenService;

    public UsersController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }
    
    [HttpGet("roles")]
    [Authorize(Roles = nameof(Roles.Administrador))]
    public async Task<IActionResult> AllRoles()
    {
        var roles = Enum.GetValues(typeof(Roles))
            .Cast<Roles>()
            .Select(role => EnumUtil.GetEnumDescription(role))
            .ToList();
        
        return Ok(roles);
    }

    [HttpGet("list")]
    [Authorize(Roles = $"{nameof(Roles.Administrador)},{nameof(Roles.UsuarioGestor)}")]
    public async Task<IActionResult> List()
    {
        var userRole = User.FindFirst("role")?.Value;
        var userClient = User.FindFirst("client")?.Value;
        return Ok(await _userService.GetAllUsersAsync(userRole, userClient));
    }
    
    [HttpPost("register")]
    [Authorize(Roles = $"{nameof(Roles.Administrador)},{nameof(Roles.UsuarioGestor)}")]
    public async Task<IActionResult> Register(RegistrarUsuarioRequest usuarioRequest)
    {
        
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        User u = await _userService.RegisterUserAsync(usuarioRequest, userRole);
        
        if (u.IsActive)
        {
            AuthResponse response = new AuthResponse(true, string.Empty, "Registro criado com sucesso!");
            return Ok(response);
        }
        else
        {
            AuthResponse response = new AuthResponse(false, string.Empty, u.Name);
            if (u.Name.Equals("Não é possível registrar um Administrador!")) return Forbid();
            return Unauthorized(response);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        User? u = await _userService.LoginUserAsync(request);
        if (u == null) return Unauthorized(new AuthResponse(false, string.Empty,"Login não autorizado."));

        string token = await _tokenService.GenerateJwtToken(u);
        AuthResponse response = new AuthResponse(true, token, "Login autorizado com sucesso!");
        return Ok(response);
    }
    
}