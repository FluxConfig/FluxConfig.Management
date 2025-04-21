using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Keys;

// Getting configuration id through header
public record CreateConfigurationApiKeyRequest(
    UserConfigRole RolePermission,
    DateTimeOffset ExpirationDate
);