namespace FluxConfig.Management.Domain.Contracts.Dal.Entities;

public class UserCredentialsEntity
{
    public long Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}