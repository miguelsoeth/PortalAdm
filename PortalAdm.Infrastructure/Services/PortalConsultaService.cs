using System.Net;
using System.Text;
using System.Text.Json;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Exceptions;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Infrastructure.Services;

public class PortalConsultaService : IPortalConsultaService
{
    private readonly HttpClient _http;

    public PortalConsultaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ConsultaOnlineResult> ConsultaOnline(ConsultaOnlineMessage request)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("http://portalconsulta:8080/api/Consulta/online", jsonContent);
        
        if (!response.IsSuccessStatusCode)
            throw new DefaultException($"Pedido falhou com StatusCode {response.StatusCode}", HttpStatusCode.BadRequest);

        var responseContent = await response.Content.ReadAsStringAsync();
        ConsultaOnlineResult? result = JsonSerializer.Deserialize<ConsultaOnlineResult>(responseContent);
        
        if (result == null)
            throw new DefaultException("Consulta sem resultados", HttpStatusCode.BadRequest);

        return result;
    }

    public async Task ConsultaLote(ConsultaLoteMessage request)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("http://portalconsulta:8080/api/Consulta/lote", jsonContent);

        if (!response.IsSuccessStatusCode)
            throw new DefaultException($"Pedido falhou com StatusCode {response.StatusCode}", response.StatusCode);
    }
}
