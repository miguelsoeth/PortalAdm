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
        
        var client = new Client(clienteRequest.Name, clienteRequest.Document, 0);
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
    
    public async Task<AuthResponse> IncreaseCredit(Guid id, decimal value)
    {
        string result = await _clientRepository.IncreaseCredit(id, value);

        if (result.Equals(String.Empty))
        {
            return new AuthResponse(true, string.Empty, "Crédito adicionado!");
        }
        
        return new AuthResponse(false, string.Empty, result);
    }
    
    public async Task<AuthResponse> DecreaseCredit(Guid id, decimal value)
    {
        string result = await _clientRepository.DecreaseCredit(id, value);

        if (result.Equals(String.Empty))
        {
            return new AuthResponse(true, string.Empty, "Crédito diminuído!");
        }
        
        return new AuthResponse(false, string.Empty, result);
    }
}