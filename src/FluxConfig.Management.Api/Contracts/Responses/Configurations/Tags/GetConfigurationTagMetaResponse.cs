using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Configurations.Tags;

public record GetConfigurationTagMetaResponse(
    long Id,
    string Tag,
    string Description,
    UserConfigRole RequiredRole
);