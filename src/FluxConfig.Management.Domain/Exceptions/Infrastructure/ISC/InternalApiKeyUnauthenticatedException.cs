namespace FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

public class InternalApiKeyUnauthenticatedException: Exception
{
    public string InvalidApiKey { get; init; }
    
    public InternalApiKeyUnauthenticatedException(string? message, string apiKey) : base(message)
    {
        InvalidApiKey = apiKey;
    }

    public InternalApiKeyUnauthenticatedException(string? message, string apiKey, Exception? innerException) : base(message, innerException)
    {
        InvalidApiKey = apiKey;
    }
}