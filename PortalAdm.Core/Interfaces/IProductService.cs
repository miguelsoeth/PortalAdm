using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IProductService
{
    Task<AuthResponse> AddProductAsync(RegistrarProdutoRequest productRequest);

    Task<IEnumerable<Product>> GetAllProductsAsync(string userRole, string userClient);
    
    Task<Product?> GetProductById(Guid id);
}