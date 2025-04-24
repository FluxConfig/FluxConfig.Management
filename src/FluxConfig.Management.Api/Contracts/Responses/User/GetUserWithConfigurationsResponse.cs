using FluxConfig.Management.Api.Contracts.Responses.Configurations.General;
using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.User;

public record GetUserWithConfigurationsResponse(
    long UserId,
    string Username,
    string Email,
    UserGlobalRole Role,
    IEnumerable<GetUserConfigurationMetaResponse> Configurations
);