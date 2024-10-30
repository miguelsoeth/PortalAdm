using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.Entities;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UsersController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }
    
    [HttpGet("roles")]
    [Authorize(Roles = $"{Roles.Administrador},{Roles.UsuarioGestor}")]
    public async Task<ActionResult<string[]>> AllRoles()
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
    public async Task<ActionResult<AuthResponse>> List()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userClient = User.FindFirst("client")?.Value;

        if (userRole == null || userClient == null )
            return BadRequest(new AuthResponse(false, string.Empty, "Erro ao obter Role/Cliente do usuário!"));
            
        return Ok(await _userService.GetAllUsersAsync(userRole, userClient));
    }
    
    [HttpPost("register")]
    [Authorize(Roles = $"{Roles.Administrador},{Roles.UsuarioGestor}")]
    public async Task<ActionResult<AuthResponse>> Register(RegistrarUsuarioRequest request)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userClient = User.FindFirst("client")?.Value;
        
        try
        {
            await _userService.RegisterUserAsync(request.Name, request.Email, request.Password, request.Role, request.ClientId, userRole, userClient);
            return Ok(new AuthResponse(true, String.Empty, "Usuário registrado com sucesso!"));
        }
        catch (DefaultException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode((int)e.Result, new AuthResponse(false, string.Empty, e.Reason));
        }
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            User u = await _userService.LoginUserAsync(request.Email, request.Password);
            string token = await _tokenService.GenerateJwtToken(u);
            return Ok(new AuthResponse(true, token, "Login autorizado com sucesso!"));
        }
        catch (DefaultException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode((int)e.Result, new AuthResponse(false, string.Empty, e.Reason));
        }
    }
    
}