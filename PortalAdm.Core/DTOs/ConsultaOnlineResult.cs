using Newtonsoft.Json;

namespace PortalAdm.Core.DTOs;

public class ConsultaOnlineResult
{
    [JsonProperty("Result")]
    public string Result { get; set; }

    public ConsultaOnlineResult(string result)
    {
        Result = result;
    }
}