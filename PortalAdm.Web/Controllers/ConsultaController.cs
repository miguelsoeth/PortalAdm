using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
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

    [HttpPost("consulta-online")]
    [Authorize]
    public async Task<IActionResult> Register(ConsultaOnlineRequest consultaRequest)
    {
        var userMail = User.FindFirst(ClaimTypes.Email)?.Value;
        AuthResponse response = await _consultaService.ConsultarOnline(consultaRequest, userMail);

        if (response.success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}