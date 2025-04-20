using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Entities;

public class UserConfigurationEntity
{
    public long UserId { get; init; }
    public UserConfigRole Role { get; init; }
    public long ConfigurationId { get; init; }
}