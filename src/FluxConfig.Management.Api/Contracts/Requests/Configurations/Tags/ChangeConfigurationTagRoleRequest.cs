using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Tags;

// Getting configuration id through header
public record ChangeConfigurationTagRoleRequest(
    long TagId,
    UserConfigRole NewRole
);