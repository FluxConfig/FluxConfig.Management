using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FluxConfig.Management.Infrastructure.DependencyInjection.Extensions;

public static class HostExtensions
{
    public static IHost MigrateUp(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();

        return host;
    }
    
    public static IHost MigrateDown(this IHost host, long migrationVersion = 20250320)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateDown(migrationVersion);

        return host;
    }
}