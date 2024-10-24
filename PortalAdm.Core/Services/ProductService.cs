using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<AuthResponse> AddProductAsync(RegistrarProdutoRequest productRequest)
    {
        if (string.IsNullOrWhiteSpace(productRequest.Name))
        {
            return new AuthResponse(false, string.Empty, "Nome é obrigatório");
        }
        
        if (productRequest.Providers.Count == 0)
        {
            return new AuthResponse(false, string.Empty, "Provedores vazio!");
        }
        
        if (productRequest.Price.Equals(null) || productRequest.Price <= 0 )
        {
            return new AuthResponse(false, string.Empty, "Preço inválido!");
        }
        
        if (productRequest.ClientId.Equals(null))
        {
            return new AuthResponse(false, string.Empty, "ClientId é obrigatório");
        }
        
        var product = new Product(productRequest.Name, productRequest.Providers, productRequest.Price, productRequest.ClientId);
        string result = await _productRepository.AddAsync(product);
        
        if (result.Equals(string.Empty))
        {
            return new AuthResponse(true, string.Empty, "Produto adicionado com sucesso!");
        }
        
        return new AuthResponse(false, string.Empty, result);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(string userRole, string userClient)
    {
        if (userRole.Equals(Roles.Administrador))
        {
            return await _productRepository.GetAllAsync();            
        }
        return await _productRepository.GetForClientAsync(userClient);
    }
}