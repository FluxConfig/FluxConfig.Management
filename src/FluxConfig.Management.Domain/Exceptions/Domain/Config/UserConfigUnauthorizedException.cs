namespace FluxConfig.Management.Domain.Exceptions.Domain.Config;

public class UserConfigUnauthorizedException: DomainException
{
    public string Reason { get;}
    
    public UserConfigUnauthorizedException(string? message, string reason) : base(message)
    {
        Reason = reason;
    }

    public UserConfigUnauthorizedException(string? message, string reason, Exception? innerException) : base(message, innerException)
    {
        Reason = reason;
    }
}