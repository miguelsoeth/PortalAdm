using Newtonsoft.Json;

namespace PortalAdm.Core.DTOs;

public class ConsultaOnlineResponse
{
    [JsonProperty("Result")]
    public string Result { get; set; }

    public ConsultaOnlineResponse(string result)
    {
        Result = result;
    }
}