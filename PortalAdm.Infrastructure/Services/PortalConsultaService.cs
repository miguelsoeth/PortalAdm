using System.Text;
using System.Text.Json;
using PortalAdm.Core.DTOs;
using PortalAdm.Core.Interfaces;

namespace PortalAdm.Infrastructure.Services;

public class PortalConsultaService : IPortalConsultaService
{
    private readonly HttpClient _http;

    public PortalConsultaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ConsultaOnlineResponse> ConsultaOnline(ConsultaOnlineMessage request)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _http.PostAsync("http://portalconsulta:8080/api/Consulta/online", jsonContent);
            
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ConsultaOnlineResponse>(responseContent);
    }

    public async Task ConsultaLote(ConsultaOnlineMessage request)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _http.PostAsync("http://portalconsulta:8080/api/Consulta/lote", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }
    }
}