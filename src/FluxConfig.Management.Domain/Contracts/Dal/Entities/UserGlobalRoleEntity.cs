using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Entities;

public class UserGlobalRoleEntity
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public UserGlobalRole Role { get; init; }
}