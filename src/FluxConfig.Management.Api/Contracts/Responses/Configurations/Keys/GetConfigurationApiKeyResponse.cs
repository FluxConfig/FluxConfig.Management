using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Configurations.Keys;

public record GetConfigurationApiKeyResponse(
    string Id,
    UserConfigRole RolePermission,
    DateTimeOffset ExpirationDate
);