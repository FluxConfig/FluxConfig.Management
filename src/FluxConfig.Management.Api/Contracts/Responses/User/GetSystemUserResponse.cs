using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.User;

public record GetSystemUserResponse(
    long Id,
    string Email,
    string Username,
    UserGlobalRole Role
);