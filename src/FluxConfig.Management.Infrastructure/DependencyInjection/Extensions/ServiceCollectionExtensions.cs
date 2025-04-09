using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Infrastructure.Configuration;
using FluxConfig.Management.Infrastructure.Configuration.Options.Enums;
using FluxConfig.Management.Infrastructure.Dal.Infrastructure;
using FluxConfig.Management.Infrastructure.Dal.Repositories;
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
        return services;
    }
}