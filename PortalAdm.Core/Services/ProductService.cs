using System.Net;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task AddProductAsync(string name, List<Guid> providers, Guid clientId, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DefaultException("Nome é obrigatório", HttpStatusCode.BadRequest);
        
        if (providers.Count == 0)
            throw new DefaultException("Provedores vazio!", HttpStatusCode.BadRequest);
        
        if (price.Equals(null) || price <= 0 )
            throw new DefaultException("Preço informado inválido!", HttpStatusCode.BadRequest);
        
        if (clientId.Equals(null))
            throw new DefaultException("ID do Cliente é obrigatório", HttpStatusCode.BadRequest);
        
        var product = new Product(name, providers, price, clientId);
        await _productRepository.AddAsync(product);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(string userRole, string userClient)
    {
        if (userRole.Equals(Roles.Administrador))
        {
            return await _productRepository.GetAllAsync();
        }
        return await _productRepository.GetForClientAsync(userClient);
    }

    public async Task<Product> GetProductById(Guid id)
    {
        Product p = await _productRepository.GetByIdAsync(id);

        if (p == null)
            throw new DefaultException("Produto não encontrado!", HttpStatusCode.BadRequest);

        return p;
    }
}