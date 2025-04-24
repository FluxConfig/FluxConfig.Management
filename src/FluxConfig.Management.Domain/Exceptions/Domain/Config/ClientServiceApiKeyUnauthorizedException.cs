namespace FluxConfig.Management.Domain.Exceptions.Domain.Config;

public class ClientServiceApiKeyUnauthorizedException: DomainException
{
    public string ApiKey { get; }
    
    public ClientServiceApiKeyUnauthorizedException(string? message, string apiKey) : base(message)
    {
        ApiKey = apiKey;
    }

    public ClientServiceApiKeyUnauthorizedException(string? message, string apiKey, Exception? innerException) : base(message, innerException)
    {
        ApiKey = apiKey;
    }
}