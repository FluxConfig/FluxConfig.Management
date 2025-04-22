using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;
using FluxConfig.Management.Domain.Models.Configuration;

namespace FluxConfig.Management.Domain.Mappers.Configuration;

internal static class EntityMapper
{
    internal static ConfigurationKeyModel MapEntityToModel(this ConfigurationKeyEntity entity)
    {
        return new ConfigurationKeyModel(
            Id: entity.Id,
            ConfigurationId: entity.ConfigurationId,
            RolePermission: entity.RolePermission,
            ExpirationDate: entity.ExpirationDate
        );
    }

    internal static IEnumerable<ConfigurationKeyModel> MapEntitiesToModelsEnumerable(
        this IReadOnlyList<ConfigurationKeyEntity> entities)
    {
        return entities.Select(e => e.MapEntityToModel());
    }

    internal static ConfigurationUsersViewModel MapEntityToModel(this ConfigurationUserViewEntity entity)
    {
        return new ConfigurationUsersViewModel(
            UserId: entity.UserId,
            ConfigurationId: entity.ConfigurationId,
            Role: entity.Role,
            Username: entity.Username,
            Email: entity.Email
        );
    }
    
    internal static IReadOnlyList<ConfigurationUsersViewModel> MapEntitiesToModels(this IReadOnlyList<ConfigurationUserViewEntity> entities)
    {
        return entities.Select(e => e.MapEntityToModel()).ToList();
    }
}