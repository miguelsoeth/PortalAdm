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

    public async Task<AuthResponse> RegisterClientAsync(RegistrarClienteRequest clienteRequest)
    {
        if (string.IsNullOrWhiteSpace(clienteRequest.Name))
        {
            return new AuthResponse(false, string.Empty, "Nome é obrigatório");
        }
        
        if (string.IsNullOrWhiteSpace(clienteRequest.Document))
        {
            return new AuthResponse(false, string.Empty, "Documento é obrigatório");
        }
        
        var client = new Client(clienteRequest.Name, clienteRequest.Document, 0);
        string result = await _clientRepository.AddAsync(client);
        
        if (result.Equals(string.Empty))
        {
            return new AuthResponse(true, string.Empty, "Cliente adicionado com sucesso!");
        }
        
        return new AuthResponse(false, string.Empty, result);
        
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