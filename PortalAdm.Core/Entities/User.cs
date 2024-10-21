using PortalAdm.Core.Enums;
using PortalAdm.SharedKernel.Security;

namespace PortalAdm.Core.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        public Roles Role { get; private set; }

        
        public User(string name, string email, string passwordHash, Roles role)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
        
        public void Activate()
        {
            IsActive = true;
        }

        public void ChangeRole(Roles role)
        {
            Role = role;
        }

        public void ChangePassword(string newPassword, IPasswordHasher passwordHasher)
        {
            PasswordHash = passwordHasher.HashPassword(newPassword);
        }

        public void UpdateUserName(string name)
        {
            Name = name;
        }
        
        public void UpdateUserEmail(string email)
        {
            Email = email;
        }
    }
}