using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
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
    public async Task<IActionResult> Add(RegistrarProdutoRequest produtoRequest)
    {
        AuthResponse response = await _productService.AddProductAsync(produtoRequest);
        
        if (response.success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userClient = User.FindFirst("client")?.Value;

        if (userRole == null || userClient == null )
            return BadRequest(new AuthResponse(false, string.Empty, "Erro ao obter Role/Cliente do usuário!"));
        
        IEnumerable<Product> p = await _productService.GetAllProductsAsync(userRole, userClient);
        return Ok(p);
    }
}