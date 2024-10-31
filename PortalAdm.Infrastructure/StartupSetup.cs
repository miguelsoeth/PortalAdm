using DadosPub.SharedKernel.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PortalAdm.Core.Interfaces;
using PortalAdm.Core.Services;
using PortalAdm.Infrastructure.Data;
using PortalAdm.Infrastructure.Repository;
using PortalAdm.Infrastructure.Services;
using PortalAdm.SharedKernel.Util;

namespace PortalAdm.Infrastructure;

public static class StartupSetup
{
    public static void AddInfrastructuresServices(this IServiceCollection services)
    {
        services.AddFeaturesServices();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<IPortalConsultaService, PortalConsultaService>();
    }
    
    public static void AddFeaturesServices(this IServiceCollection services)
    {
        services.AddJwt(
            AmbienteUtil.GetValue("JWT_ISSUER"),
            AmbienteUtil.GetValue("JWT_AUDIENCE"), 
            AmbienteUtil.GetValue("JWT_KEY")
        );
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(AmbienteUtil.GetValue("POSTGRES_CONNECTION")));
    }
}