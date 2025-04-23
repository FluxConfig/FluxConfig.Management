namespace FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

public class FcStorageNotFoundException: InfrastructureException
{
    public string Key { get; }
    public string Tag { get; }
    
    public FcStorageNotFoundException(string? message, string key, string tag, Exception? innerException) : base(message, innerException)
    {
        Key = key;
        Tag = tag;
    }
}