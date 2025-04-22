namespace FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

public class FcStorageInternalApiKeyUnauthenticatedException: InfrastructureException
{
    public string InvalidApiKey { get; }
    
    public FcStorageInternalApiKeyUnauthenticatedException(string? message, string invalidApiKey, Exception? innerException) : base(message, innerException)
    {
        InvalidApiKey = invalidApiKey;
    }
}