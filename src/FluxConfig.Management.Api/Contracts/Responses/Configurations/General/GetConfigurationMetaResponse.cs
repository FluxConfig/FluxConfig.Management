using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Configurations.General;

public record GetConfigurationMetaResponse(
    long Id,
    string Name,
    string Description,
    UserConfigRole UserRole
);