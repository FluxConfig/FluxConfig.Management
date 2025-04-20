using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Entities;

public class ConfigurationTagEntity
{
    public long Id { get; init; }
    public long ConfigurationId { get; init; }
    public string Tag { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public UserConfigRole RequiredRole { get; init; }
}