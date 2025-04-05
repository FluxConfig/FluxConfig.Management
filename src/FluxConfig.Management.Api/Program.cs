namespace FluxConfig.Management.Api;

public sealed class Program
{
    public static async Task Main()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        await hostBuilder
            .Build()
            .RunAsync();
    }
}