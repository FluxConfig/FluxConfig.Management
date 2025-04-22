namespace FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

public class InternalApiKeyUnauthenticatedException: InfrastructureException
{
    public string InvalidApiKey { get; }
    
    public InternalApiKeyUnauthenticatedException(string? message, string apiKey) : base(message)
    {
        InvalidApiKey = apiKey;
    }

    public InternalApiKeyUnauthenticatedException(string? message, string apiKey, Exception? innerException) : base(message, innerException)
    {
        InvalidApiKey = apiKey;
    }
}