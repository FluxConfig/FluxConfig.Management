namespace FluxConfig.Management.Infrastructure.Configuration.Options;

public class PostgreCredentialsOptions
{
    public string Id { get; init; }
    public string Password { get; init; }

    public PostgreCredentialsOptions(string id, string password)
    {
        Id = id;
        Password = password;
    }
}