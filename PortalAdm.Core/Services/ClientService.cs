using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Core.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Client> RegisterClientAsync(RegistrarClienteRequest clienteRequest)
    {
        if (string.IsNullOrWhiteSpace(clienteRequest.Name))
        {
            return new Client("Nome é obrigatório");
        }
        
        if (string.IsNullOrWhiteSpace(clienteRequest.Document))
        {
            return new Client("Documento é obrigatório");
        }
        
        var client = new Client(clienteRequest.Name, clienteRequest.Document);
        string success = await _clientRepository.AddAsync(client);
        
        if (success.Equals(string.Empty))
        {
            return client;
        }
        
        return new Client(success);
        
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await _clientRepository.GetAllAsync();
    }
}