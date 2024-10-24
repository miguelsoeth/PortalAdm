using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.Entities;
using PortalAdm.Core.DTOs;
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
    [Authorize(Roles = $"{Roles.Administrador},{Roles.UsuarioGestor}")]
    public async Task<IActionResult> AllRoles()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userRole == null)
            return BadRequest(new AuthResponse(false, String.Empty, "Erro ao adquirir Role do usuário atual!"));

        if (userRole.Equals(Roles.UsuarioGestor))
        {
            return Ok(Roles.GetPublicRoles());
        }
        
        return Ok(Roles.GetAllRoles());
    }

    [HttpGet("list")]
    [Authorize(Roles = $"{Roles.Administrador},{Roles.UsuarioGestor}")]
    public async Task<IActionResult> List()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userClient = User.FindFirst("client")?.Value;

        if (userRole == null || userClient == null )
        {
            AuthResponse response = new AuthResponse(false, string.Empty, "Erro ao obter Role/Cliente do usuário!");
            return BadRequest(response);
        }
            
        return Ok(await _userService.GetAllUsersAsync(userRole, userClient));
    }
    
    [HttpPost("register")]
    [Authorize(Roles = $"{Roles.Administrador},{Roles.UsuarioGestor}")]
    public async Task<IActionResult> Register(RegistrarUsuarioRequest usuarioRequest)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userClient = User.FindFirst("client")?.Value;
        
        AuthResponse response = await _userService.RegisterUserAsync(usuarioRequest, userRole, userClient);
        
        if (response.success)
        {
            return Ok(response);
        }

        if (response is { success: false, token: "Forbid" })
        {
            return Forbid();
        }
        
        return Unauthorized(response);
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