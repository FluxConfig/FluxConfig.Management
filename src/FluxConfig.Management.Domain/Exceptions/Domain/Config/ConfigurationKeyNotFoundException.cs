namespace FluxConfig.Management.Domain.Exceptions.Domain.Config;

public class ConfigurationKeyNotFoundException: DomainException
{
    public string KeyId { get; }
    
    public ConfigurationKeyNotFoundException(string? message, string keyId) : base(message)
    {
        KeyId = keyId;
    }

    public ConfigurationKeyNotFoundException(string? message, string keyId, Exception? innerException) : base(message, innerException)
    {
        KeyId = keyId;
    }
}
