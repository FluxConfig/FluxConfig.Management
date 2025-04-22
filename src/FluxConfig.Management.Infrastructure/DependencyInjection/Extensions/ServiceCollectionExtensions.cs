using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Infrastructure.Configuration;
using FluxConfig.Management.Infrastructure.Configuration.Options.Enums;
using FluxConfig.Management.Infrastructure.Dal.Infrastructure;
using FluxConfig.Management.Infrastructure.Dal.Repositories;
using FluxConfig.Management.Infrastructure.ISC.Clients;
using FluxConfig.Management.Infrastructure.ISC.Clients.Interfaces;
using FluxConfig.Management.Infrastructure.ISC.GrpcContracts.Storage;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxConfig.Management.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalInfrastructure(this IServiceCollection services,
        IConfiguration configuration, bool isDevelopment)
    {
        string appConnectionString =
            ConfigurationGetter.GetPostgreConnectionString(
                configuration: configuration,
                connectionUser: PostgreUserType.App,
                isDevelopment: isDevelopment
            );


        string migrationsConnectionString =
            ConfigurationGetter.GetPostgreConnectionString(
                configuration: configuration,
                connectionUser: PostgreUserType.Migrations,
                isDevelopment: isDevelopment
            );

        Postgres.AddDataSource(services, appConnectionString, isDevelopment);
        Postgres.ConfigureTypeMapOptions();
        Postgres.AddMigrations(services, migrationsConnectionString);


        return services;
    }

    public static IServiceCollection AddDalRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionsRepository, SessionsRepository>();

        services.AddScoped<IConfigurationsRepository, ConfigurationsRepository>();
        services.AddScoped<IUserConfigurationRepository, UserConfigurationRepository>();
        services.AddScoped<IConfigurationKeysRepository, ConfigurationKeysRepository>();
        services.AddScoped<IConfigurationTagsRepository, ConfigurationTagsRepository>();

        return services;
    }

    public static IServiceCollection AddStorageClient(this IServiceCollection services, IConfiguration configuration,
        bool isDevelopment)
    {
        services.AddScoped<IFluxConfigStorageClient, FluxConfigStorageClient>();

        string fluxConfigStorageAddress = ConfigurationGetter.GetFcsBaseUrl(
            configuration: configuration,
            isDevelopment: isDevelopment
        );
        string fcInternalApiKey = ConfigurationGetter.GetInternalFcApiKey(
            configuration: configuration,
            isDevelopment: isDevelopment
        );

        var retryMethodConfig = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 3,
                InitialBackoff = TimeSpan.FromMilliseconds(500),
                MaxBackoff = TimeSpan.FromSeconds(2),
                BackoffMultiplier = 1.3,
                RetryableStatusCodes = { StatusCode.Unavailable }
            }
        };

        services
            .AddGrpcClient<Storage.StorageClient>(options =>
            {
                options.Address = new Uri(fluxConfigStorageAddress);
            })
            .AddCallCredentials((context, metadata) =>
            {
                metadata.Add("X-API-KEY", fcInternalApiKey);
                return Task.CompletedTask;
            })
            .ConfigureChannel(options =>
            {
                options.ServiceConfig = new ServiceConfig
                {
                    MethodConfigs = { retryMethodConfig }
                };
            });


        return services;
    }
}