using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetForClientAsync(string userClient);
    Task<string> AddAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
}