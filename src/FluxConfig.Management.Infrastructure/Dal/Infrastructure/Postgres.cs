using FluentMigrator.Runner;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.NameTranslation;

namespace FluxConfig.Management.Infrastructure.Dal.Infrastructure;

public static class Postgres
{
    private static readonly INpgsqlNameTranslator Translator = new NpgsqlSnakeCaseNameTranslator();

    public static void ConfigureTypeMapOptions()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public static void AddDataSource(IServiceCollection services, string appConnectionString,
        bool isDevelopment)
    {
        services.AddNpgsqlDataSource(
            connectionString: appConnectionString,
            builder =>
            {
                builder.MapComposite<UserCredentialsEntity>("user_credentials_type", Translator);
                builder.MapComposite<UserSessionEntity>("user_session_type", Translator);
                builder.MapEnum<UserGlobalRole>("user_global_role_enum");
                if (isDevelopment)
                {
                    builder.EnableParameterLogging();
                }
            }
        );
    }

    public static void AddMigrations(IServiceCollection services, string migrationsConnectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(r => r
                .AddPostgres()
                .WithGlobalConnectionString(connectionStringOrName: migrationsConnectionString)
                .ScanIn(typeof(Postgres).Assembly).For.Migrations()
            );
    }
}