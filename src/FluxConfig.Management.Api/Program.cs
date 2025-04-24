using System.Net;
using FluxConfig.Management.Infrastructure.DependencyInjection.Extensions;

namespace FluxConfig.Management.Api;

public sealed class Program
{
    public static async Task Main()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder.ConfigureKestrel((context, serverOptions) =>
                    {
                        serverOptions.Listen(IPAddress.Any, 7070);
                    }
                );
            });

        await hostBuilder
            .Build()
            .MigrateUp()
            .RunAsync();
    }
}