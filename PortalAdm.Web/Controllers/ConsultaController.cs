using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsultaController : ControllerBase
{
    private readonly IConsultaService _consultaService;

    public ConsultaController(IConsultaService consultaService)
    {
        _consultaService = consultaService;
    }

    [HttpPost("online")]
    [Authorize]
    public async Task<ActionResult<ConsultaOnlineResult>> Online(ConsultaOnlineRequest request)
    {
        if (User.IsInRole(Roles.Administrador))
            return Forbid();
        
        try
        {
            var userMail = User.FindFirst(ClaimTypes.Email)?.Value;
            ConsultaOnlineResult response = await _consultaService.ConsultarOnline(request.ProductId, request.Document, userMail);
            return response;
        }
        catch (DefaultException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode((int)e.Result, new AuthResponse(false, string.Empty, e.Reason));
        }
    }
    
    [HttpPost("lote")]
    [Authorize]
    public async Task<IActionResult> Lote(Guid ProductId, IFormFile file)
    {
        if (User.IsInRole(Roles.Administrador))
            return Forbid();
        
        var userMail = User.FindFirst(ClaimTypes.Email)?.Value;

        if (file.ContentType == "text/xml")
        {
            return Ok(file.ContentType);
        }
        
        return BadRequest(file.ContentType);
    }
}