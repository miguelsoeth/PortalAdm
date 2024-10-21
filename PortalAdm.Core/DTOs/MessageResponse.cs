namespace PortalAdm.Core.DTOs;

public class MessageResponse(string message)
{
    public string Message { get; set; } = message;
}