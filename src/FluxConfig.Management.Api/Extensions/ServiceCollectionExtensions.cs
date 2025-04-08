using FluxConfig.Management.Api.FiltersAttributes;

namespace FluxConfig.Management.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddGlobalFilters(this IServiceCollection services)
    {
        services.AddScoped<ExceptionFilter>();
        return services;
    }
}