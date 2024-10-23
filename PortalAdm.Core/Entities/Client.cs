namespace PortalAdm.Core.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public decimal Credit { get; private set; }
    
    public Client(string message)
    {
        Id = Guid.Empty;
        Name = message;
        Document = string.Empty;
        Credit = 0;
    }

    public Client(string name, string document, decimal credit)
    {
        Id = Guid.NewGuid();
        Name = name;
        Document = document;
        Credit = credit;
    }
    
    public Client(Guid id, string name, string document, decimal credit)
    {
        Id = id;
        Name = name;
        Document = document;
        Credit = credit;
    }
    
    public void UpdateUserName(string name)
    {
        Name = name;
    }
        
    public void UpdateDocument(string document)
    {
        Document = document;
    }
    
    public void UpdateInfos(string name, string document)
    {
        Name = name;
        Document = document;
    }

    public void IncreaseCredit(decimal value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("O valor a aumentar deve ser maior que zero.");
        }
        
        Credit += value;
    }
    
    public void DecreaseCredit(decimal value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("O valor a diminuir deve ser maior que zero.");
        }

        if (Credit <= 0 || Credit < value)
        {
            throw new InvalidOperationException("Crédito insuficiente para diminuir.");
        }

        Credit -= value;
    }
}