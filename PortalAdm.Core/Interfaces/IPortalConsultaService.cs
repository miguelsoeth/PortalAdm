using PortalAdm.Core.DTOs;

namespace PortalAdm.Core.Interfaces;

public interface IPortalConsultaService
{
    Task<ConsultaOnlineResult> ConsultaOnline(ConsultaOnlineMessage request);
    Task ConsultaLote(ConsultaOnlineMessage request);
}