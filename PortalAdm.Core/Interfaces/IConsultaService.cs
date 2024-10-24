using PortalAdm.Core.DTOs;

namespace PortalAdm.Core.Interfaces;

public interface IConsultaService
{
    Task<AuthResponse> ConsultarOnline(ConsultaOnlineRequest consultaRequest, string? userMail);
}