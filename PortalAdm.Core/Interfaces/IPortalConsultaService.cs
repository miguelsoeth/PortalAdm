using PortalAdm.Core.DTOs;

namespace PortalAdm.Core.Interfaces;

public interface IPortalConsultaService
{
    Task<ConsultaOnlineResponse> ConsultaOnline(ConsultaOnlineMessage request);
    Task ConsultaLote(ConsultaOnlineMessage request);
}