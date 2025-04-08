using FluxConfig.Management.Domain.Exceptions.Infrastructure;

namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class UserNotFoundException: DomainException
{
    public long Id { get; init; } = -1;
    public string? InvalidEmail { get; init; }
    public UserNotFoundException(string? message, string? invalidEmail,  EntityNotFoundException? innerException) : base(message, innerException)
    {
        InvalidEmail = invalidEmail;
    }
    
    public UserNotFoundException(string? message, string? invalidEmail, long id,  EntityNotFoundException? innerException) : base(message, innerException)
    {
        InvalidEmail = invalidEmail;
        Id = id;
    }
}