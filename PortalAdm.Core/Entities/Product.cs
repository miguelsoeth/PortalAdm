namespace PortalAdm.Core.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public List<Guid> Providers { get; private set; }
    public decimal Price { get; private set; }
    public Guid ClientId { get; private set; }
    
    public Product(string name, List<Guid> providers, decimal price, Guid clientId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Providers = providers;
        Price = price;
        ClientId = clientId;
    }
    
    public void UpdateName(string name)
    {
        Name = name;
    }

    public void SetProviders(List<Guid> providers)
    {
        Providers = providers;
    }
    
    public void AddProvider(Guid provider)
    {
        Providers.Add(provider);
    }
    
    public void RemoveProvider(Guid provider)
    {
        Providers.Remove(provider);
    }

    public void setPrice(decimal price)
    {
        Price = price;
    }
}