using System.Net;

namespace PortalAdm.Core.Exceptions;

public class DefaultException : Exception
{
    public string Reason { get; }
    public HttpStatusCode Result { get; }
    
    public DefaultException(string reason, HttpStatusCode result) 
        : base($"Erro no pedido: {reason}")
    {
        Reason = reason;
        Result = result;
    }
    
    public override string ToString()
    {
        return $"{base.ToString()}, Reason: {Reason}, HTTP Status Code: {Result}";
    }

}