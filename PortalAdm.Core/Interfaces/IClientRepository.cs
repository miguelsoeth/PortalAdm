using PortalAdm.Core.Entities;

namespace PortalAdm.Core.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetById(Guid id);
    Task<IEnumerable<Client>> GetAllAsync();
    Task<string> AddAsync(Client client);
}