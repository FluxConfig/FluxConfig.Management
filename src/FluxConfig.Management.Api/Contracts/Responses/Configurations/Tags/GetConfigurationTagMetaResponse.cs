using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Configurations.Tags;

public record GetConfigurationTagMetaResponse(
    long Id,
    long ConfigurationId,
    string Tag,
    string Description,
    string ConfigurationName,
    UserConfigRole RequiredRole
);