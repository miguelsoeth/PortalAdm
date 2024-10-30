namespace PortalAdm.Core.DTOs;

public class RegistrarProdutoRequest
{
    public string Name { get; set; }
    public List<Guid> Providers { get; set; }
    public Guid ClientId { get; set; }
    public decimal Price { get; set; }

}