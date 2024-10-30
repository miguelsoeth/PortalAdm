using System.Net;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Core.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task RegisterClientAsync(string name, string document)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DefaultException("Nome é obrigatório", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(document))
            throw new DefaultException("Documento é obrigatório", HttpStatusCode.BadRequest);

        var client = new Client(name, document, 0);
        await _clientRepository.AddAsync(client);
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await _clientRepository.GetAllAsync();
    }
    
    public async Task IncreaseCredit(Guid id, decimal value)
    {
        Client c = await _clientRepository.GetById(id);
        await _clientRepository.IncreaseCredit(c, value);
    }
    
    public async Task DecreaseCredit(Guid id, decimal value)
    {
        Client c = await _clientRepository.GetById(id);
        await _clientRepository.DecreaseCredit(c, value);
    }
}