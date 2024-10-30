using PortalAdm.Core.DTOs;
using PortalAdm.Core.Models;

namespace PortalAdm.Core.Interfaces;

public interface IConsultaService
{
    Task<ConsultaOnlineResult> ConsultarOnline(Guid productId, string document, string? userMail);
    Task ConsultarLote(Guid productId, FileModel file, string? userMail);
}