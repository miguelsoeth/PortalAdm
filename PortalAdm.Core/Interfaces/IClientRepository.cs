using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IClientRepository
{
    Task<Client> GetById(Guid id);
    Task<IEnumerable<Client>> GetAllAsync();
    Task AddAsync(Client client);
    Task IncreaseCredit(Client client, decimal value);
    Task DecreaseCredit(Client client, decimal value);
}