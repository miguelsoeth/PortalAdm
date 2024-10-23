using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
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
    public async Task<IActionResult> Register(RegistrarClienteRequest clienteRequest)
    {
        Client c = await _clientService.RegisterClientAsync(clienteRequest);
        
        if (c.Id.Equals(Guid.Empty))
        {
            AuthResponse response = new AuthResponse(false, string.Empty, c.Name);
            return Unauthorized(response);
        }
        else
        {
            AuthResponse response = new AuthResponse(true, string.Empty, "Registro criado com sucesso!");
            return Ok(response);
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
    public async Task<IActionResult> Increase(Guid id, decimal value)
    {
        //value = Math.Round(value, 2);
        AuthResponse response = await _clientService.IncreaseCredit(id, value);
        if (response.success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPost("credit/decrease")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Decrease(Guid id, decimal value)
    {
        //value = Math.Round(value, 2);
        AuthResponse response = await _clientService.DecreaseCredit(id, value);
        if (response.success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}