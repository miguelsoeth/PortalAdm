using Microsoft.Extensions.DependencyInjection;
using PortalAdm.Core.Interfaces;
using PortalAdm.Core.Services;

namespace PortalAdm.Core;

public static class StartupSetup
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IClientService, ClientService>();
    }
}