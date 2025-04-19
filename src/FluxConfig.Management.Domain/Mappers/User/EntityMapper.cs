using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Domain.Mappers.User;

internal static class EntityMapper
{
    internal static UserModel MapEntityToModel(this UserCredentialsEntity entity)
    {
        return new UserModel(
            Id: entity.Id,
            Username: entity.Username,
            Email: entity.Email,
            Password: entity.Password,
            Role: entity.Role
        );
    }
}