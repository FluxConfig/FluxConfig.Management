using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Hasher;
using FluxConfig.Management.Domain.Models.Auth;

namespace FluxConfig.Management.Domain.Mappers.User;

internal static class ModelMapper
{
    internal static UserCredentialsEntity MapModelToEntity(this UserRegisterModel model)
    {
        return new UserCredentialsEntity
        {
            Id = -1,
            Username = model.Username,
            Email = model.Email,
            Password = PasswordHasher.Hash(model.Password)
        };
    }
}