namespace PortalAdm.Core.DTOs;

public class UserListResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}