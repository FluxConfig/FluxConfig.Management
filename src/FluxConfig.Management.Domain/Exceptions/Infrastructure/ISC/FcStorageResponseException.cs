namespace FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;

public class FcStorageResponseException: InfrastructureException
{
    public int StatusCode { get; }
    
    public FcStorageResponseException(string? message, int statusCode, Exception? innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

}