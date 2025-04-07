namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class IncorrectUserCredentialsException: DomainException
{
    public IncorrectUserCredentialsException()
    {
    }

    public IncorrectUserCredentialsException(string? message) : base(message)
    {
    }

    public IncorrectUserCredentialsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}