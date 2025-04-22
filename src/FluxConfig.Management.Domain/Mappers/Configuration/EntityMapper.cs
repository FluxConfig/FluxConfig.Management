using FluxConfig.Management.Domain.Contracts.Dal.Entities;
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
    
    internal static IEnumerable<ConfigurationKeyModel> mapEntitiesToModelsEnumerable(this IReadOnlyList<ConfigurationKeyEntity> entities)
    {
        return entities.Select(e => e.MapEntityToModel());
    }
}