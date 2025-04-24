using FluxConfig.Management.Domain.Services;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Management.Domain.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IConfigurationsDataService, ConfigurationsDataService>();
        services.AddScoped<IConfigurationsMetaService, ConfigurationsMetaService>();
        services.AddScoped<IConfigurationKeysService, ConfigurationKeysService>();
        services.AddScoped<IConfigurationTagsService, ConfigurationTagsService>();
        services.AddScoped<IConfigurationUsersService, ConfigurationUsersService>();

        return services;
    }
}