using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Configurations.Tags;

public record GetConfigurationTagResponse(
    long Id,
    string Tag,
    UserConfigRole RequiredRole
);