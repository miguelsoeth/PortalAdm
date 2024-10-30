using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;
using PortalAdm.Core.Models;

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
    public async Task<IActionResult> Lote(ConsultaLoteRequest request)
    {
        if (User.IsInRole(Roles.Administrador))
            return Forbid();
        
        try
        {
            var userMail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (request.File.ContentType != "text/csv")
                throw new DefaultException("Tipo de arquivo não aceito!", HttpStatusCode.BadRequest);
            
            var file = new FileModel
            {
                FileStream = request.File.OpenReadStream(),
                FileName = request.File.FileName,
                ContentType = request.File.ContentType
            };

            await _consultaService.ConsultarLote(request.ProductId, file, userMail);
            
            return Ok();
        }
        catch (DefaultException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode((int)e.Result, new AuthResponse(false, string.Empty, e.Reason));
        }
    }
}