using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IClientService
{
    Task<Client> RegisterClientAsync(RegistrarClienteRequest clienteRequest);

    Task<IEnumerable<Client>> GetAllClientsAsync();
}