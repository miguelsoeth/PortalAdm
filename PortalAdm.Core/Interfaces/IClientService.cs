using PortalAdm.Core.DTOs;
using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IClientService
{
    Task RegisterClientAsync(string name, string document);

    Task<IEnumerable<Client>> GetAllClientsAsync();

    Task IncreaseCredit(Guid id, decimal value);
    Task DecreaseCredit(Guid id, decimal value);
}
