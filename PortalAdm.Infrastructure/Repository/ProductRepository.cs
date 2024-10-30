using System.Net;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;
using PortalAdm.Infrastructure.Data;

namespace PortalAdm.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetForClientAsync(string userClient)
    {
        Guid userClientId = new Guid(userClient);
        
        return await _context.Products
            .Where(product => product.ClientId == userClientId)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(u => u.Name == product.Name && u.ClientId == product.ClientId);
        if (existingProduct != null)
        {
            throw new DefaultException("Um produto com esse nome já existe!", HttpStatusCode.BadRequest);
        }
        
        await _context.Products.AddAsync(product);
        int result = await _context.SaveChangesAsync();
        if (result <= 0)
        {
            throw new DefaultException("Não foi possível adicionar o produto!", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }
}