namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class UserUnauthenticatedException: DomainException
{
    public UserUnauthenticatedException()
    {
    }

    public UserUnauthenticatedException(string? message) : base(message)
    {
    }

    public UserUnauthenticatedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}