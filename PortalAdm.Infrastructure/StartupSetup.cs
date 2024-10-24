using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PortalAdm.Core.Interfaces;
using PortalAdm.Infrastructure.Data;
using PortalAdm.Infrastructure.Identity;
using PortalAdm.Infrastructure.Repository;
using PortalAdm.SharedKernel.Security;
using PortalAdm.SharedKernel.Util;

namespace PortalAdm.Infrastructure;

public static class StartupSetup
{
    public static void AddInfrastructuresServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
    }
    
    public static void AddDbContext(this IServiceCollection services) {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(AmbienteUtil.GetValue("POSTGRES_CONNECTION")));
    }
    
    public static void ConfigureJwt(this IServiceCollection services) => JwtStartupSetup.RegisterJWT(services);
}