using FluxConfig.Management.Domain.Exceptions.Infrastructure;

namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class UserNotFoundException: DomainException
{
    public UserNotFoundException()
    {
    }

    public UserNotFoundException(string? message) : base(message)
    {
    }

    public UserNotFoundException(string? message, EntityNotFoundException? innerException) : base(message, innerException)
    {
    }
}