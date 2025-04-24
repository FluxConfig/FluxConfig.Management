using FluentMigrator.Runner;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
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
                builder.MapEnum<UserGlobalRole>("user_global_role_enum");
                builder.MapEnum<UserConfigRole>("user_config_role_enum");
                
                builder.MapComposite<UserCredentialsEntity>("user_credentials_type", Translator);
                builder.MapComposite<UserSessionEntity>("user_session_type", Translator);
                
                builder.MapComposite<ConfigurationEntity>("configuration_type", Translator);
                builder.MapComposite<UserConfigurationEntity>("user_configurations_type", Translator);
                builder.MapComposite<ConfigurationTagEntity>("configuration_tag_type", Translator);
                builder.MapComposite<ConfigurationKeyEntity>("configuration_key_type", Translator);

                builder.MapComposite<ConfigurationUserViewEntity>("configuration_user_view_type", Translator);
                builder.MapComposite<ConfigurationTagsViewEntity>("configuration_tags_view_type", Translator);
                builder.MapComposite<UserConfigurationsViewEntity>("user_configurations_view_type", Translator);
                    
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