using Microsoft.EntityFrameworkCore;
using Npgsql;
using PortalAdm.Core.Entities;
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

    public async Task<string> AddAsync(Product product)
    {
        try
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(u => u.Name == product.Name && u.ClientId == product.ClientId);
            if (existingProduct != null)
            {
                throw new Exception("Um produto com esse nome já existe!");
            }
            
            await _context.Products.AddAsync(product);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return string.Empty;
            }
            
            throw new Exception("Erro ao registrar produto!");
        }
        catch (Exception ex)
        {
            if (ex.InnerException is PostgresException e)
            {
                if (e.SqlState == PostgresErrorCodes.ForeignKeyViolation)
                    return $"Chave estrangeira {e.ConstraintName} incorreta!";
                
                return e.Message;
            }
            
            return ex.Message;
        }
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }
}