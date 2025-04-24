namespace FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

public class FcStorageAlreadyExistsException: InfrastructureException
{
    public string Tag { get; }
    
    public FcStorageAlreadyExistsException(string? message, string tag, Exception? innerException) : base(message, innerException)
    {
        Tag = tag;
    }
}