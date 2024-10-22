namespace PortalAdm.Core.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    
    public Client(string message)
    {
        Id = Guid.Empty;
        Name = message;
        Document = string.Empty;
    }

    public Client(string name, string document)
    {
        Id = Guid.NewGuid();
        Name = name;
        Document = document;
    }
    
    public Client(Guid id, string name, string document)
    {
        Id = id;
        Name = name;
        Document = document;
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
}