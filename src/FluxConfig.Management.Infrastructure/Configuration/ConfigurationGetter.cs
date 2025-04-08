using FluxConfig.Management.Infrastructure.Configuration.Options;
using FluxConfig.Management.Infrastructure.Configuration.Options.Enums;
using Microsoft.Extensions.Configuration;

namespace FluxConfig.Management.Infrastructure.Configuration;

public static class ConfigurationGetter
{
    private const string PgAppUserEnvKey = "PG_FCM_APP_USER";
    private const string PgAppPasswordEnvKey = "PG_FCM_APP_PSWD";
    private const string PgMigrationUserEnvKey = "PG_MIGRATION_USER";
    private const string PgMigrationPasswordEnvKey = "PG_MIGRATION_PSWD";
    private const string FcsUrlEnvKey = "FCS_BASE_URL";
    private const string FcApiKeyEnvKey = "FC_API_KEY";

    public static string GetInternalFcApiKey(IConfiguration configuration, bool isDevelopment)
    {
        if (isDevelopment)
        {
            return configuration.GetValue<string>("ISC:InternalApiKey") ??
                   throw new ArgumentException("Internal FC api key is missing");
        }

        return GetEnvVariable(FcApiKeyEnvKey);
    }

    internal static string GetFcsBaseUrl(IConfiguration configuration, bool isDevelopment)
    {
        if (isDevelopment)
        {
            return configuration.GetValue<string>("ISC:FcsBaseUrl") ??
                   throw new ArgumentException("FC Storage base url is missing");
        }

        return GetEnvVariable(FcsUrlEnvKey);
    }

    internal static string GetPostgreConnectionString(IConfiguration configuration, PostgreUserType connectionUser,
        bool isDevelopment)
    {
        var postgreConnectionOptionsSection = configuration.GetSection("PostgreOptions:ConnectionOptions");

        PostgreConnectionOptions connectionOptions = postgreConnectionOptionsSection.Get<PostgreConnectionOptions>() ??
                                                     throw new ArgumentException(
                                                         "Postgre connection options are missing.");

        PostgreCredentialsOptions credentialsOptions = isDevelopment
            ? GetDevelopmentPostgreConnectionCredentials(configuration, connectionUser)
            : GetProdPostgreConnectionCredentials(connectionUser);

        return
            $"USER ID={credentialsOptions.Id};Password={credentialsOptions.Password};Host={connectionOptions.Host};Port={connectionOptions.Port};Database={connectionOptions.Database};Pooling=true";
    }

    private static PostgreCredentialsOptions GetDevelopmentPostgreConnectionCredentials(IConfiguration configuration,
        PostgreUserType connectionUser)
    {
        IConfigurationSection credentialsConfigurationSection = connectionUser == PostgreUserType.App
            ? configuration.GetSection("PostgreOptions:Credentials:App")
            : configuration.GetSection("PostgreOptions:Credentials:Migrations");

        return credentialsConfigurationSection.Get<PostgreCredentialsOptions>()
               ?? throw new ArgumentException(
                   $"Postgre credentials options for user {connectionUser.ToString()} are missing.");
    }


    private static PostgreCredentialsOptions GetProdPostgreConnectionCredentials(PostgreUserType connectionUser)
    {
        if (connectionUser == PostgreUserType.App)
        {
            return new PostgreCredentialsOptions(
                id: GetEnvVariable(PgAppUserEnvKey),
                password: GetEnvVariable(PgAppPasswordEnvKey)
            );
        }

        return new PostgreCredentialsOptions(
            id: GetEnvVariable(PgMigrationUserEnvKey),
            password: GetEnvVariable(PgMigrationPasswordEnvKey)
        );
    }


    private static string GetEnvVariable(string envKey)
    {
        return Environment.GetEnvironmentVariable(envKey) ??
               throw new ArgumentException($"Required environment variable with key {envKey} is missing.");
    }
}