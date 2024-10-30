using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    [HttpPost("register")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<AuthResponse>> Register(RegistrarClienteRequest request)
    {
        try
        {
            await _clientService.RegisterClientAsync(request.Name, request.Document);
            return Ok(new AuthResponse(true, string.Empty, "Cliente registrado com sucesso!"));
        }
        catch (DefaultException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode((int)e.Result, new AuthResponse(false, string.Empty, e.Reason));
        }
    }
    
    [HttpGet("list")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> List()
    {
        IEnumerable<Client> c = await _clientService.GetAllClientsAsync();
        return Ok(c);
    }
    
    [HttpPost("credit/increase")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<AuthResponse>> Increase(Guid id, decimal value)
    {
        try
        {
            await _clientService.IncreaseCredit(id, value);
            return Ok(new AuthResponse(true, string.Empty, "Crédito do cliente aumentado com sucesso!"));
        }
        catch (DefaultException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode((int)e.Result, new AuthResponse(false, string.Empty, e.Reason));
        }
    }
    
    [HttpPost("credit/decrease")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<AuthResponse>> Decrease(Guid id, decimal value)
    {
        try
        {
            await _clientService.DecreaseCredit(id, value);
            return Ok(new AuthResponse(true, string.Empty, "Crédito do cliente aumentado com sucesso!"));
        }
        catch (DefaultException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode((int)e.Result, new AuthResponse(false, string.Empty, e.Reason));
        }
    }
}