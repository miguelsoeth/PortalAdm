using Microsoft.EntityFrameworkCore;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Interfaces;
using PortalAdm.Infrastructure.Data;

namespace PortalAdm.Infrastructure.Repository;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetById(Guid id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<string> AddAsync(Client client)
    {
        try
        {
            var existingUser = await _context.Clients.FirstOrDefaultAsync(u => u.Document == client.Document);
            if (existingUser != null)
            {
                return "Um cliente com esse documento já existe!";
            }
            await _context.Clients.AddAsync(client);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return string.Empty;
            }

            return "Erro ao registrar cliente!";
        }
        catch (Exception ex)
        {
            return $"Erro ao adicionar cliente: {ex.Message}";
        }
    }
    
    public async Task<string> IncreaseCredit(Guid id, decimal value)
    {
        try
        {
            var client = await GetById(id);
            if (client == null) throw new Exception("Não foi possível encontrar o cliente");
            
            client.IncreaseCredit(value);
            
            _context.Clients.Update(client);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return string.Empty;
            }

            throw new Exception("Não foi possível atualizar o crédito do cliente");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    
    public async Task<string> DecreaseCredit(Guid id, decimal value)
    {
        try
        {
            var client = await GetById(id);
            if (client == null) throw new Exception("Não foi possível encontrar o cliente");
            
            client.DecreaseCredit(value);
            
            _context.Clients.Update(client);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return string.Empty;
            }

            throw new Exception("Não foi possível atualizar o crédito do cliente");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}