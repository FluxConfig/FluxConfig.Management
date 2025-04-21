using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Configurations.General;

public record GetUserConfigurationMetaResponse(
    long Id,
    string Name,
    UserConfigRole UserRole
);