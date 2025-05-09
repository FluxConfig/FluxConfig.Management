using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Auth;

public record UserCheckAuthResponse(
    long Id,
    string Email,
    string Username,
    UserGlobalRole Role
);