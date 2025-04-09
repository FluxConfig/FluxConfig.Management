using FluxConfig.Management.Api.FiltersAttributes;
using FluxConfig.Management.Api.FiltersAttributes.Auth;
using FluxConfig.Management.Infrastructure.Configuration;
using static System.Int32;

namespace FluxConfig.Management.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddGlobalFilters(this IServiceCollection services)
    {
        services.AddScoped<ExceptionFilter>();
        services.AddScoped<ApiKeyAuthFilter>();
        return services;
    }

    internal static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration,
        bool isDevelopment)
    {
        string fcWcUrl = ConfigurationGetter.GetFcWcUrl(
            configuration: configuration,
            isDevelopment: isDevelopment);

        bool portSelected = TryParse(fcWcUrl.Split(':', StringSplitOptions.RemoveEmptyEntries)[^1], out var clientPort);

        string[] clientOrigins =
        [
            $"http://host.docker.internal{(portSelected ? $":{clientPort}" : "")}",
            $"http://172.17.0.1{(portSelected ? $":{clientPort}" : "")}",
            fcWcUrl
        ];

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(clientOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}