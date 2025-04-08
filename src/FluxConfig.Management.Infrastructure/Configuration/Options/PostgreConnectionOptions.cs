namespace FluxConfig.Management.Infrastructure.Configuration.Options;

public class PostgreConnectionOptions
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string Database { get; init; } = string.Empty;
}