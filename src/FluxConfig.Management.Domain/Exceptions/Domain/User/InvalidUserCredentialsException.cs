namespace FluxConfig.Management.Domain.Exceptions.Domain.User;

public class InvalidUserCredentialsException: DomainException
{
    public string Email { get; init; }
    public string? InvalidPassword { get; init; }

    public InvalidUserCredentialsException(string? message, string email, string? invalidPassword) : base(message)
    {
        Email = email;
        InvalidPassword = invalidPassword;
    }
}