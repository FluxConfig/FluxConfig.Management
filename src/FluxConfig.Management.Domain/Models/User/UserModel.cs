using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.User;

public record UserModel(
    long Id,
    string Username,
    string Email,
    string Password,
    IEnumerable<UserGlobalRole> Roles
);