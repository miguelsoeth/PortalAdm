using Microsoft.EntityFrameworkCore;
using PortalAdm.Core.Entities;
using PortalAdm.Core.Enums;

namespace PortalAdm.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}
    
    private readonly Guid _admClientId = Guid.NewGuid();
    
    public DbSet<Client> Clients { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("tbl_clientes");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Document).IsRequired();
            
            entity.HasData(
                new Client(_admClientId, "DEPS", "00000000000000")
            );
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("tbl_usuarios");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.IsActive).IsRequired();
            entity.Property(u => u.Role).IsRequired();
            
            entity.HasIndex(u => u.Email).IsUnique();
            
            entity.HasOne<Client>()
                .WithMany()
                .HasForeignKey(u => u.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasData(
                new User("Admin", "admin@mail", "LxbyMDmF9PLnsdIgMNb4hw==:RCcI4rpkmM/kaZGyTXk1RD1h86z5fYHrhOPIny1QsUg=", Roles.Administrador, _admClientId)
            );
        });
    }
}