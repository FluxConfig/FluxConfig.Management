namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class UserUnauthenticatedException: DomainException
{
    public string Reason { get; init; }
    public UserUnauthenticatedException(string? message, string reason,  Exception? innerException) : base(message, innerException)
    {
        Reason = reason;
    }
    
    public UserUnauthenticatedException(string? message, string reason) : base(message)
    {
        Reason = reason;
    }
}