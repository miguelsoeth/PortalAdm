namespace PortalAdm.Core.DTOs;

public class AuthResponse
{
    public bool success { get; set; }
    public string message { get; set; }
    public string token { get; set; }
    
    public AuthResponse(bool success, string token, string message)
    {
        this.success = success;
        this.token = token;
        this.message = message;
    }
}