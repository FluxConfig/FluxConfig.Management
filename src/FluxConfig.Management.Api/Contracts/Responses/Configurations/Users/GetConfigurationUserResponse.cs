using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Configurations.Users;

public record GetConfigurationUserResponse(
    long Id,
    string Username,
    string Email,
    UserConfigRole Role
);