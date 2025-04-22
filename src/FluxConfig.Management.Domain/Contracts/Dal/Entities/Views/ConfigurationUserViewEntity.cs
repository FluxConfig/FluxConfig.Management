using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;

public class ConfigurationUserViewEntity
{
    public long UserId { get; init; }
    public long ConfigurationId { get; init; }
    public UserConfigRole Role { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}