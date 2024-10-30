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
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("add")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<AuthResponse>> Add(RegistrarProdutoRequest request)
    {
        try
        {
            await _productService.AddProductAsync(request.Name, request.Providers, request.ClientId, request.Price);
            return Ok(new AuthResponse(true, String.Empty, "Produto adicionado com sucesso!"));
        }
        catch (DefaultException e)
        {
            Console.WriteLine(e.ToString());
            return StatusCode((int)e.Result, new AuthResponse(false, string.Empty, e.Reason));
        }
    }
    
    [HttpGet("list")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Product>>> List()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userClient = User.FindFirst("client")?.Value;

        if (userRole == null || userClient == null )
            return BadRequest(new AuthResponse(false, string.Empty, "Erro ao obter Role/Cliente do usuário!"));
        
        IEnumerable<Product> p = await _productService.GetAllProductsAsync(userRole, userClient);
        return Ok(p);
    }
}