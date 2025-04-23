namespace FluxConfig.Management.Domain.Exceptions.Domain.Config;

public class ConfigurationTagAlreadyExistsException: DomainException
{
    public string Tag { get; }
    public long ConfigurationId { get; }
    
    public ConfigurationTagAlreadyExistsException(string? message, string tag, long configurationId, Exception? innerException) : base(message, innerException)
    {
        Tag = tag;
        ConfigurationId = configurationId;
    }
}