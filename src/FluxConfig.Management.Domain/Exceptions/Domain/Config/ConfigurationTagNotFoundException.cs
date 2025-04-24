namespace FluxConfig.Management.Domain.Exceptions.Domain.Config;

public class ConfigurationTagNotFoundException: DomainException
{
    public long TagId { get; }
    
    public ConfigurationTagNotFoundException(string? message, long tagId) : base(message)
    {
        TagId = tagId;
    }

    public ConfigurationTagNotFoundException(string? message, long tagId, Exception? innerException) : base(message, innerException)
    {
        TagId = tagId;
    }
}