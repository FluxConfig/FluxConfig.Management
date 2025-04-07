using FluxConfig.Management.Domain.Exceptions.Infrastructure;

namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class UserAlreadyExistsException: DomainException
{
    public UserAlreadyExistsException()
    {
    }

    public UserAlreadyExistsException(string? message) : base(message)
    {
    }

    public UserAlreadyExistsException(string? message, EntityAlreadyExistsException? innerException) : base(message, innerException)
    {
    }
}