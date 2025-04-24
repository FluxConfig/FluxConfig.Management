namespace FluxConfig.Management.Domain.Contracts.Dal.Entities;

public class UserSessionEntity
{
    public string Id { get; init; } = string.Empty;
    public long UserId { get; init; }
    public DateTimeOffset ExpirationDate { get; init; }
}