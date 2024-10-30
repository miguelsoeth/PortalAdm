using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IProductService
{
    Task AddProductAsync(string name, List<Guid> providers, Guid clientId, decimal price);

    Task<IEnumerable<Product>> GetAllProductsAsync(string userRole, string userClient);
    
    Task<Product> GetProductById(Guid id);
}