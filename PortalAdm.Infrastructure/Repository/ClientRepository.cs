using System.Net;
using Microsoft.EntityFrameworkCore;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
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

    public async Task<Client> GetById(Guid id)
    {
        Client? c = await _context.Clients.FindAsync(id);

        if (c == null)
            throw new DefaultException("Cliente não encontrado!", HttpStatusCode.BadRequest);

        return c;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task AddAsync(Client client)
    {
        var existingUser = await _context.Clients.FirstOrDefaultAsync(u => u.Document == client.Document);
        
        if (existingUser != null)
            throw new DefaultException("Um cliente com esse documento já existe!", HttpStatusCode.BadRequest);
        
        await _context.Clients.AddAsync(client);
        int result = await _context.SaveChangesAsync();
        if (result <= 0)
        {
            throw new DefaultException("Erro ao registrar cliente!", HttpStatusCode.BadRequest);
        }
    }
    
    public async Task IncreaseCredit(Client client, decimal value)
    {
        client.IncreaseCredit(value);
        _context.Clients.Update(client);
        int result = await _context.SaveChangesAsync();
        
        if (result <= 0)
            throw new DefaultException("Não foi possível atualizar o crédito do cliente", HttpStatusCode.InternalServerError);
    }
    
    public async Task DecreaseCredit(Client client, decimal value)
    {
        if (client == null)
            throw new DefaultException("Não foi possível encontrar o cliente", HttpStatusCode.BadRequest);
        
        client.DecreaseCredit(value);
        _context.Clients.Update(client);
        int result = await _context.SaveChangesAsync();
        
        if (result <= 0)
            throw new DefaultException("Não foi possível atualizar o crédito do cliente", HttpStatusCode.InternalServerError);
    }
}