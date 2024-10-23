using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetById(Guid id);
    Task<IEnumerable<Client>> GetAllAsync();
    Task<string> AddAsync(Client client);
    Task<string> IncreaseCredit(Guid id, decimal value);
    Task<string> DecreaseCredit(Guid id, decimal value);
}