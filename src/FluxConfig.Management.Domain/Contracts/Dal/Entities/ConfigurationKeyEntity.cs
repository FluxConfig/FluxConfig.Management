using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Entities;

public class ConfigurationKeyEntity
{
    public string Id { get; init; } = string.Empty;
    public long ConfigurationId { get; init; }
    public UserConfigRole RolePermission { get; init; }
    public DateTimeOffset ExpirationDate { get; init; }
}