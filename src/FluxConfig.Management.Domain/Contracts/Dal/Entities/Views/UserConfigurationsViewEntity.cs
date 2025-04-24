using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;

public class UserConfigurationsViewEntity
{
    public long UserId { get; init; }
    public long ConfigurationId { get; init; }
    public UserConfigRole Role { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}