using FluxConfig.Management.Domain.Services;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Management.Domain.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserCredentialsService, UserCredentialsService>();

        return services;
    }
}