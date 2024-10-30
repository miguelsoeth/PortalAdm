using PortalAdm.Core.DTOs;

namespace PortalAdm.Core.Interfaces;

public interface IConsultaService
{
    Task<ConsultaOnlineResult> ConsultarOnline(Guid productId, string document, string? userMail);
}