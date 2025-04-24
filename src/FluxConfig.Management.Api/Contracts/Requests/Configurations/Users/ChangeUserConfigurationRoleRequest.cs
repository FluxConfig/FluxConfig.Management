using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Users;

// Getting configuration id through header
public record ChangeUserConfigurationRoleRequest(
    long UserId,
    UserConfigRole NewRole
);