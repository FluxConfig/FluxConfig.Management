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

    internal static IReadOnlyList<ConfigurationUsersViewModel> MapEntitiesToModels(
        this IReadOnlyList<ConfigurationUserViewEntity> entities)
    {
        return entities.Select(e => e.MapEntityToModel()).ToList();
    }

    internal static UserConfigurationsViewModel MapEntityToModel(this UserConfigurationsViewEntity entity)
    {
        return new UserConfigurationsViewModel(
            UserId: entity.UserId,
            ConfigurationId: entity.ConfigurationId,
            Role: entity.Role,
            ConfigurationName: entity.Name,
            ConfigurationDescription: entity.Description
        );
    }

    internal static IReadOnlyList<UserConfigurationsViewModel> MapEntitiesToModels(
        this IReadOnlyList<UserConfigurationsViewEntity> entities)
    {
        return entities.Select(e => e.MapEntityToModel()).ToList();
    }

    private static ConfigurationTagModel MapEntityToModel(this ConfigurationTagEntity entity)
    {
        return new ConfigurationTagModel(
            Id: entity.Id,
            ConfigurationId: entity.ConfigurationId,
            Tag: entity.Tag,
            Description: entity.Description,
            RequiredRole: entity.RequiredRole
        );
    }

    internal static IEnumerable<ConfigurationTagModel> MapEntitiesToModels(
        this IReadOnlyList<ConfigurationTagEntity> entities)
    {
        return entities.Select(e => e.MapEntityToModel());
    }

    internal static ConfigurationTagsViewModel MapEntityToModel(this ConfigurationTagsViewEntity entity)
    {
        return new ConfigurationTagsViewModel(
            ConfigurationId: entity.ConfigurationId,
            Name: entity.Name,
            StorageKey: entity.StorageKey,
            ConfigurationDescription: entity.ConfigurationDescription,
            TagId: entity.TagId,
            TagName: entity.TagName,
            RequiredRole: entity.RequiredRole,
            TagDescription: entity.TagDescription
        );
    }
}