using FluxConfig.Management.Domain.Exceptions.Infrastructure;

namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class UserAlreadyExistsException: DomainException
{
    public string Email { get; init; }

    public UserAlreadyExistsException(string? message, string email, EntityAlreadyExistsException? innerException) : base(message, innerException)
    {
        Email = email;
    }
}