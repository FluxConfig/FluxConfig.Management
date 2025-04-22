namespace FluxConfig.Management.Domain.Exceptions.Domain.Config;

public class ConfigurationNotFoundException: DomainException
{
    public long ConfigurationId { get; }
    
    public ConfigurationNotFoundException(string? message, long configurationId) : base(message)
    {
        ConfigurationId = configurationId;
    }

    public ConfigurationNotFoundException(string? message, long configurationId, Exception? innerException) : base(message, innerException)
    {
        ConfigurationId = configurationId;
    }
}