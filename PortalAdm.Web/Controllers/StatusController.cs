using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.Entities;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    private IUserService _userService;
    private ITokenService _tokenService;

    public StatusController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }
    
    [HttpGet]
    public IActionResult Status()
    {
        return Ok(new MessageResponse("Serviço ok!."));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrarRequest request)
    {
        User u = await _userService.RegisterUserAsync(request);
        return Ok(u);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Register(LoginRequest request)
    {
        User? u = await _userService.LoginUserAsync(request);
        if (u == null) return Unauthorized(new MessageResponse("Login não autorizado."));

        string token = await _tokenService.GenerateJwtToken(u);
        return Ok(token);
    }
    
}