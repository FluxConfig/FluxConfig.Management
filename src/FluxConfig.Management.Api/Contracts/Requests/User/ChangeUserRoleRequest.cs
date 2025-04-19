using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Requests.User;

public record ChangeUserRoleRequest(
    long UserId,
    UserGlobalRole Role
);