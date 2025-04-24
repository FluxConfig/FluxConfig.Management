using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.Configuration;

public record ConfigurationTagModel(
    long Id,
    long ConfigurationId,
    string Tag,
    string Description,
    UserConfigRole RequiredRole
);