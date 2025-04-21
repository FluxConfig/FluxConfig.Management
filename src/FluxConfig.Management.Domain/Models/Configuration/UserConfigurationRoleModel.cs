using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.Configuration;

public record UserConfigurationRoleModel(
    long UserId,
    UserConfigRole Role,
    long ConfigurationId
);