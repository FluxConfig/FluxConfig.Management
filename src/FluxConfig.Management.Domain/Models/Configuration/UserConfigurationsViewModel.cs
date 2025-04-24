using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.Configuration;

public record UserConfigurationsViewModel(
    long UserId,
    long ConfigurationId,
    UserConfigRole Role,
    string ConfigurationName,
    string ConfigurationDescription
);