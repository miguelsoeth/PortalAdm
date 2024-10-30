namespace PortalAdm.Core.DTOs;

public class ConsultaLoteRequest
{
    public Guid ProductId { get; set; }
    public IFormFile File { get; set; }
}