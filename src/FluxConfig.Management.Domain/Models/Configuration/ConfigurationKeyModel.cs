using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.Configuration;

public record ConfigurationKeyModel(
    string Id,
    long ConfigurationId,
    UserConfigRole RolePermission,
    DateTimeOffset ExpirationDate
);